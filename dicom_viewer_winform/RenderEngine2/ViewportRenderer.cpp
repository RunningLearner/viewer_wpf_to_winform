#include <Windows.h>
#include "ViewportRenderer.h"
#include <algorithm>
#include "Conversions.h"

#include <vtkAutoInit.h>
VTK_MODULE_INIT(vtkRenderingOpenGL2)
VTK_MODULE_INIT(vtkRenderingVolumeOpenGL2)

#include <vtkWin32OpenGLRenderWindow.h>
#include <vtkWindowToImageFilter.h>
#include <vtkSphereSource.h>
#include <vtkWin32OutputWindow.h>
#include <vtkCamera.h>
#include <vtkCoordinate.h>

#define VTI_FILETYPE 1
#define MHA_FILETYPE 2

using namespace RenderEngine2;
using namespace System::Data::Linq;

class RenderEngine2::ViewportRendererImpl
{
public:
    ViewportRendererImpl()
		: renderWindow(vtkSmartPointer<vtkRenderWindow>::New())
        , renderer(vtkSmartPointer<vtkOpenGLRenderer>::New())
		, windowToImageFilter(vtkSmartPointer<vtkWindowToImageFilter>::New())
    {
        /*vtkSmartPointer<vtkWin32OutputWindow> outputWindow = vtkSmartPointer<vtkWin32OutputWindow>::New();
        outputWindow->SetSendToStdErr(true);
        vtkOutputWindow::SetInstance(outputWindow);*/
        
        renderWindow->AddRenderer(this->renderer);
        renderWindow->DebugOn();

        renderWindow->SetOffScreenRendering(true);

        renderWindow->SetSize(400, 400);

        windowToImageFilter->SetInputBufferTypeToRGBA();
        windowToImageFilter->SetInput(this->renderWindow);

        vtkCamera* camera = renderer->GetActiveCamera();
        camera->ParallelProjectionOn();
        camera->SetParallelScale(100);
    }

    ~ViewportRendererImpl()
    {
        windowToImageFilter = nullptr;
        renderWindow = nullptr;
        renderer = nullptr;
    }
  
    void SetSize(int width, int height)
    {
        renderWindow->SetSize(width, height);
    }

    void SetCameraTransformation(double up[3], double position[3], double focalPoint[3])
    {
        vtkCamera* camera = renderer->GetActiveCamera();
        camera->SetViewUp(up[0], up[1], up[2]);
        camera->SetPosition(position[0], position[1], position[2]);
        camera->SetFocalPoint(focalPoint[0], focalPoint[1], focalPoint[2]);
    }

    void SetZoom(double factor)
    {
        vtkCamera *camera = renderer->GetActiveCamera();
        camera->SetParallelScale(factor);
    }

    void Render(byte* pixelData)
    {
        renderWindow->Render();

        windowToImageFilter->Modified();
        windowToImageFilter->Update();

        vtkImageData* imageData = windowToImageFilter->GetOutput();
        int width = imageData->GetDimensions()[0];
        int height = imageData->GetDimensions()[1];

        unsigned char *colorsPtr =
            reinterpret_cast<unsigned char *>(imageData->GetScalarPointer());

        byte * currentRgbPtr = pixelData;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                *currentRgbPtr++ = colorsPtr[2];
                *currentRgbPtr++ = colorsPtr[1];
                *currentRgbPtr++ = colorsPtr[0];
                *currentRgbPtr++ = colorsPtr[3];
                colorsPtr += 4;
            }
        }
    }

    vtkRenderer* GetRenderer()
    {
        return renderer.GetPointer();
    }

private:
    vtkSmartPointer<vtkRenderWindow> renderWindow;
    vtkSmartPointer<vtkOpenGLRenderer> renderer;
    vtkSmartPointer<vtkWindowToImageFilter> windowToImageFilter;
};

RenderEngine2::ViewportRenderer::ViewportRenderer()
{
    impl = new ViewportRendererImpl();
}

RenderEngine2::ViewportRenderer::~ViewportRenderer()
{
    this->!ViewportRenderer();
}

RenderEngine2::ViewportRenderer::!ViewportRenderer()
{
    if (impl != nullptr)
    {
        delete impl;
        impl = nullptr;
    }
}

Vector3 ^ RenderEngine2::ViewportRenderer::GetPositionInWorld(double x, double y)
{
    vtkCoordinate* coordinate = vtkCoordinate::New();
    coordinate->SetCoordinateSystemToDisplay();
    coordinate->SetValue(x, y);

    double* world = coordinate->GetComputedWorldValue(impl->GetRenderer());
    return gcnew Vector3(world[0], world[1], world[2]);
}

void RenderEngine2::ViewportRenderer::Render(IntPtr pixelData)
{
    impl->Render((byte*)pixelData.ToPointer());
}

void RenderEngine2::ViewportRenderer::SetSize(int width, int height)
{
    impl->SetSize(width, height);
}

void RenderEngine2::ViewportRenderer::SetZoom(double factor)
{
    impl->SetZoom(factor);
}

void RenderEngine2::ViewportRenderer::SetCameraTransformation(Matrix ^ transformation)
{
    Vector3^ position = transformation * gcnew Vector3(0, 0, 0);
    Vector3^ direction = transformation * gcnew Vector3(0, 0, -400);
    Vector3^ up = (transformation * gcnew Vector3(0, 1, 0)) + -position;

    double upArr[3] = { up->X, up->Y, up->Z };
    double positionArr[3] = { direction->X, direction->Y, direction->Z };
    double focalPointArr[3]{ position->X, position->Y, position->Z };
    impl->SetCameraTransformation(upArr, positionArr, focalPointArr);
}

vtkRenderer * RenderEngine2::ViewportRenderer::GetRenderer()
{
    return impl->GetRenderer();
}
