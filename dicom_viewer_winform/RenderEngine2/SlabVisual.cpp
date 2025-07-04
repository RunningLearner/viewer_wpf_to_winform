#include "SlabVisual.h"

#include "vtkMemoryImageReader.h"
#include <vtkCamera.h>
#include <vtkSmartPointer.h>
#include <vtkImageSlice.h>
#include <vtkImageResliceMapper.h>
#include <vtkPlane.h>
#include <vtkImageProperty.h>
#include <vtkMatrix4x4.h>
#include <vtkPolyData.h>
#include <vtkCellData.h>
#include <vtkLine.h>
#include "ImageSetReaderFactory.h"

class RenderEngine2::SlabVisualPrivates 
{
public:
	vtkMemoryImageReader* imageReader;
	vtkSmartPointer<vtkImageResliceMapper> mapper;
	vtkSmartPointer<vtkImageSlice> imageSlice;
	vtkSmartPointer<vtkImageProperty> imageProperty;
};

RenderEngine2::SlabVisual::SlabVisual(Entities::ImageSet ^ images)
{
	privates = new SlabVisualPrivates();

	this->images = images;

	privates->imageReader = ImageSetReaderFactory::Acquire(images);
	
	privates->imageSlice = vtkSmartPointer<vtkImageSlice>::New();
	privates->mapper = vtkSmartPointer<vtkImageResliceMapper>::New();

	privates->mapper->SetInputData(privates->imageReader->GetOutput());
	privates->imageSlice->SetMapper(privates->mapper);

	privates->imageProperty = vtkSmartPointer<vtkImageProperty>::New();
	privates->imageProperty->SetInterpolationTypeToCubic();	
	privates->imageProperty->SetColorWindow(2000);
	privates->imageProperty->SetColorLevel(500);

	privates->imageSlice->SetProperty(privates->imageProperty);

	privates->mapper->SetSlabTypeToMax();
	privates->mapper->SliceFacesCameraOn();
	privates->mapper->SliceAtFocalPointOn();
	privates->mapper->AutoAdjustImageQualityOff();
	privates->mapper->SetSlabSampleFactor(1);

	auto rotation = images->TransformationToPatient;
	auto position = images->Slices[0]->PositionPatient;
	auto transform = Matrix::Translation(position) * rotation;

	vtkSmartPointer<vtkMatrix4x4> userMatrix = vtkSmartPointer<vtkMatrix4x4>::New();
	for (int y = 0; y < 4; y++)
	{
		for (int x = 0; x < 4; x++)
		{
			userMatrix->SetElement(x, y, transform[y * 4 + x]);
		}
	}
	privates->imageSlice->SetUserMatrix(userMatrix);
}

void RenderEngine2::SlabVisual::PreRender(ViewportRenderer^ viewport)
{

}

void RenderEngine2::SlabVisual::AddTo(ViewportRenderer ^ viewport)
{
	auto renderer = viewport->GetRenderer();
	renderer->AddViewProp(privates->imageSlice);
}

void RenderEngine2::SlabVisual::RemoveFrom(ViewportRenderer ^ viewport)
{
	auto renderer = viewport->GetRenderer();
	renderer->RemoveViewProp(privates->imageSlice);
}

double RenderEngine2::SlabVisual::GetWindowLevel()
{
	return privates->imageProperty->GetColorLevel();
}

double RenderEngine2::SlabVisual::GetWindowWidth()
{
	return privates->imageProperty->GetColorWindow();
}

void RenderEngine2::SlabVisual::SetWindowing(double level, double width)
{
	privates->imageProperty->SetColorLevel(level);
	privates->imageProperty->SetColorWindow(width);
	Invalidated();
}

double RenderEngine2::SlabVisual::GetSlabThickness()
{
	return privates->mapper->GetSlabThickness();
}

void RenderEngine2::SlabVisual::SetSlabThickness(double thicknessInMM)
{
	privates->mapper->SetSlabThickness(thicknessInMM);
	Invalidated();
}

RenderEngine2::SlabVisual::!SlabVisual()
{
	delete privates;
	ImageSetReaderFactory::Release(images);
}

RenderEngine2::SlabVisual::~SlabVisual()
{
	this->!SlabVisual();
}
