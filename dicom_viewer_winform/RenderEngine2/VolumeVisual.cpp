#include "VolumeVisual.h"

#include <vtkOpenGLGPUVolumeRayCastMapper.h>
#include <vtkColorTransferFunction.h>
#include <vtkPiecewiseFunction.h>
#include <vtkMatrix4x4.h>
#include <vtkVolumeProperty.h>
#include "vtkMemoryImageReader.h"
#include <vtkImageHistogram.h>
#include "ImageSetReaderFactory.h"

using namespace RenderEngine2;

struct RenderEngine2::VolumeVisualPrivates
{
    double windowWidth = 360;
    double windowLevel = 310;


	vtkMemoryImageReader* reader;
    vtkSmartPointer<vtkVolume> volume;
    vtkSmartPointer<vtkOpenGLGPUVolumeRayCastMapper> mapper;
    vtkSmartPointer<vtkColorTransferFunction> volumeColor;
    vtkSmartPointer<vtkPiecewiseFunction> volumeScalarOpacity;
    vtkSmartPointer<vtkVolumeProperty> volumeProperty;
    vtkSmartPointer<vtkImageHistogram> histogram;
};


VolumeVisual::VolumeVisual(ImageSet ^ volumeData)
    : privates(new VolumeVisualPrivates())
{
	images = volumeData;

	privates->reader = ImageSetReaderFactory::Acquire(volumeData);
    
    privates->volume = vtkSmartPointer<vtkVolume>::New();
    privates->mapper = vtkSmartPointer<vtkOpenGLGPUVolumeRayCastMapper>::New();
	privates->mapper->AutoAdjustSampleDistancesOff();

    privates->mapper->SetInputConnection(privates->reader->GetOutputPort());
    privates->mapper->AutoAdjustSampleDistancesOff();
    privates->mapper->SetSampleDistance(0.5);

    privates->volumeColor =
        vtkSmartPointer<vtkColorTransferFunction>::New();

    privates->volumeColor->AddRGBPoint(-2024, 0, 0, 0, 0.5, 0.0);
    privates->volumeColor->AddRGBPoint(-16, 0.73, 0.25, 0.30, 0.49, .61);
    privates->volumeColor->AddRGBPoint(641, .90, .82, .56, .5, 0.0);
    privates->volumeColor->AddRGBPoint(3071, 1, 1, 1, .5, 0.0);

    // The opacity transfer function is used to control the opacity
    // of different tissue types.
    privates->volumeScalarOpacity = vtkSmartPointer<vtkPiecewiseFunction>::New();
    //volumeScalarOpacity->AddPoint(1150, 0.85);

    //volumeScalarOpacity->AddPoint(-3024, 0, 0.5, 0.0);
    //volumeScalarOpacity->AddPoint(-16, 0, .49, .61);
    //volumeScalarOpacity->AddPoint(641, .72, .5, 0.0);
    //volumeScalarOpacity->AddPoint(3071, .71, 0.5, 0.0);

    // The gradient opacity function is used to decrease the opacity
    // in the "flat" regions of the volume while maintaining the opacity
    // at the boundaries between tissue types.  The gradient is measured
    // as the amount by which the intensity changes over unit distance.
    // For most medical data, the unit distance is 1mm.
    vtkSmartPointer<vtkPiecewiseFunction> volumeGradientOpacity =
        vtkSmartPointer<vtkPiecewiseFunction>::New();
    volumeGradientOpacity->AddPoint(0, 0.0);
    volumeGradientOpacity->AddPoint(50, 0.5);
    volumeGradientOpacity->AddPoint(100, 1.0);

    privates->volumeProperty = vtkSmartPointer<vtkVolumeProperty>::New();

    privates->volumeProperty->SetColor(privates->volumeColor);

    privates->volumeProperty->SetScalarOpacity(privates->volumeScalarOpacity);
    privates->volumeProperty->SetGradientOpacity(volumeGradientOpacity);
    privates->volumeProperty->SetInterpolationTypeToLinear();
    privates->volumeProperty->ShadeOn();
    privates->volumeProperty->SetAmbient(0.1);
    privates->volumeProperty->SetDiffuse(0.8);
    privates->volumeProperty->SetSpecular(0.2);
    privates->volumeProperty->GetSpecularPower(50);
    privates->volumeProperty->SetScalarOpacityUnitDistance(0.8919);

    privates->volume->SetProperty(privates->volumeProperty);
    privates->volume->SetMapper(privates->mapper);

    auto rotation = volumeData->TransformationToPatient;
    auto position = volumeData->Slices[0]->PositionPatient;
    auto transform = Matrix::Translation(position) * rotation;

    vtkSmartPointer<vtkMatrix4x4> userMatrix = vtkSmartPointer<vtkMatrix4x4>::New();
    for (int y = 0; y < 4; y++)
    {
        for (int x = 0; x < 4; x++)
        {
            userMatrix->SetElement(x, y, transform[y * 4 + x]);
        }
    }
    privates->volume->SetUserMatrix(userMatrix);

    SetWindowing(1000, 2000);

    privates->histogram =
        vtkSmartPointer<vtkImageHistogram>::New();
    privates->histogram->SetInputConnection(privates->reader->GetOutputPort());
}

VolumeVisual::~VolumeVisual()
{
    this->!VolumeVisual();
}

RenderEngine2::VolumeVisual::!VolumeVisual()
{
    delete privates;
	ImageSetReaderFactory::Release(images);
}

void RenderEngine2::VolumeVisual::PreRender(ViewportRenderer ^ viewport)
{
}

void VolumeVisual::AddTo(ViewportRenderer ^ viewport)
{
    viewport->GetRenderer()->AddVolume(privates->volume);
}

void RenderEngine2::VolumeVisual::RemoveFrom(ViewportRenderer ^ viewport)
{
    viewport->GetRenderer()->RemoveVolume(privates->volume);
}

array<long>^ RenderEngine2::VolumeVisual::GetHistogram()
{
    privates->histogram->GenerateHistogramImageOff();
    privates->histogram->AutomaticBinningOff();
    privates->histogram->Update();

    vtkIdType nbins = privates->histogram->GetNumberOfBins();
    double range[2];
    range[0] = privates->histogram->GetBinOrigin();
    range[1] = range[0] + (nbins - 1)*privates->histogram->GetBinSpacing();

    vtkIdTypeArray* output = privates->histogram->GetHistogram();
    const long nrValues = output->GetNumberOfValues();
    auto result = gcnew array<long>(nrValues);
    for (int i = 1; i < nrValues - 1; i++)
    {
        result[i] = Math::Sqrt(output->GetValue(i));
    }
    return result;
}

void VolumeVisual::SetWindowing(double windowLevel, double windowWidth)
{
    privates->windowLevel = windowLevel;
    privates->windowWidth = windowWidth;

    privates->volumeScalarOpacity->RemoveAllPoints();

    double min = windowLevel - windowWidth / 2;
    double max = windowLevel + windowWidth / 2;
    privates->volumeScalarOpacity->AddPoint(-2000, 0.00, 0.5, 0);
    privates->volumeScalarOpacity->AddPoint(min, 0.0, 0.5, 0.5);
    privates->volumeScalarOpacity->AddPoint(max, 1, 0.5, 0);

    Invalidated();
}

double RenderEngine2::VolumeVisual::GetWindowLevel()
{
    return privates->windowLevel;
}

double RenderEngine2::VolumeVisual::GetWindowWidth()
{
    return privates->windowWidth;
}
