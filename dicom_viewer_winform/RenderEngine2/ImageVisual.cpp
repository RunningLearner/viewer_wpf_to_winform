#include "ImageVisual.h"

#include <vtkRenderWindow.h>
#include <vtkWindowToImageFilter.h>
#include <vtkImageMapper3D.h>
#include <vtkImageActor.h>
#include <vtkImageProperty.h>
#include <vtkSmartPointer.h>
#include "vtkMemoryImageReader.h"
#include "ImageSetReaderFactory.h"

class ImageVisualPrivates
{
public:
	vtkMemoryImageReader* imageReader;
    vtkSmartPointer<vtkImageSliceMapper> mapper;
    vtkSmartPointer<vtkImageSlice> image;
    vtkSmartPointer<vtkImageProperty> imageProperty;
    vtkRenderer* renderer;
    int numberOfImages;
    int currentImageIndex;
};

RenderEngine2::ImageVisual::ImageVisual(ImageSet^ images)
{
    privates = new ImageVisualPrivates();

    this->images = images;

	privates->imageReader = ImageSetReaderFactory::Acquire(images);

    numberOfImages = images->Slices->Count;
    int width = images->Slices[0]->Width;
    int height = images->Slices[0]->Height;

    auto firstImage = images->Slices[0];

    privates->image = vtkSmartPointer<vtkImageSlice>::New();
    privates->mapper = vtkSmartPointer<vtkImageSliceMapper>::New();

    privates->mapper->SetInputData(privates->imageReader->GetOutput());
    privates->image->SetMapper(privates->mapper);

    privates->imageProperty = vtkSmartPointer<vtkImageProperty>::New();
    privates->imageProperty->SetInterpolationTypeToCubic();
    privates->image->SetProperty(privates->imageProperty);

    int index = (images->Slices->Count - 1) / 2;
    SetImageIndex(index);

    auto image = images->Slices[index];
    if (image->DefaultWindowingAvailable)
    {
        privates->imageProperty->SetColorWindow(image->WindowWidth);
        privates->imageProperty->SetColorLevel(image->WindowLevel);
    }
}

RenderEngine2::ImageVisual::!ImageVisual()
{
    delete privates;
	ImageSetReaderFactory::Release(images);
}

RenderEngine2::ImageVisual::~ImageVisual()
{
    this->!ImageVisual();
}

void RenderEngine2::ImageVisual::PreRender(ViewportRenderer^ viewport)
{
}

void RenderEngine2::ImageVisual::AddTo(ViewportRenderer ^ viewport)
{
    privates->renderer = viewport->GetRenderer();
    viewport->GetRenderer()->AddActor(privates->image);
}

void RenderEngine2::ImageVisual::RemoveFrom(ViewportRenderer ^ viewport)
{
    viewport->GetRenderer()->RemoveActor(privates->image);
}

double RenderEngine2::ImageVisual::GetWindowLevel()
{
    return privates->imageProperty->GetColorLevel();
}

double RenderEngine2::ImageVisual::GetWindowWidth()
{
    return privates->imageProperty->GetColorWindow();
}

void RenderEngine2::ImageVisual::SetWindowing(double level, double width)
{
    privates->imageProperty->SetColorLevel(level);
    privates->imageProperty->SetColorWindow(width);
	Invalidated();
}

int RenderEngine2::ImageVisual::GetImageIndex()
{
    return currentImageIndex;
}

int RenderEngine2::ImageVisual::GetNumberOfImages()
{
    return numberOfImages;
}

void RenderEngine2::ImageVisual::SetImageIndex(int index)
{
    currentImageIndex = index;
    privates->mapper->SetSliceNumber(currentImageIndex);
	Invalidated();
}
