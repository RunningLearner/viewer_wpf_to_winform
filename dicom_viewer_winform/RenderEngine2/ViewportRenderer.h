#pragma once

#include <vtkSphereSource.h>
#include <vtkOpenGLPolyDataMapper.h>
#include <vtkRenderWindow.h>
#include <vtkRenderWindowInteractor.h>
#include <vtkOpenGLRenderer.h>
#include <vtkOpenGLActor.h>
#include <vtkWindowToImageFilter.h>

using namespace System;
using namespace Entities;

namespace RenderEngine2 {

	class ViewportRendererImpl;

	public ref class ViewportRenderer: IDisposable
    {
    public:
		ViewportRenderer();
		~ViewportRenderer();
		!ViewportRenderer();

        Vector3^ GetPositionInWorld(double x, double y);

        void Render(IntPtr pixelData);
        void SetSize(int width, int height);
        void SetZoom(double factor);        
        void SetCameraTransformation(Matrix^ matrix);
	internal:
		vtkRenderer* GetRenderer();

    private:
		ViewportRendererImpl* impl;
    };
}
