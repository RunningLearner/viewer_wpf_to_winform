#pragma once

#include "IVisual.h"

namespace RenderEngine2 {
	
	class SlabAxesVisualPrivates;

    public enum class Plane
    {
        XY,
        YZ,
        XZ,
        None
    };

	public ref class SlabAxesVisual : public IVisual
	{
	public:
		SlabAxesVisual(Entities::Space^ space, Plane hiddenPlane);
		!SlabAxesVisual();
		~SlabAxesVisual();

        bool Pick(double x, double y);
		void Highlight();
		void DeHighlight();

		virtual void PreRender(ViewportRenderer^ viewport);
		virtual void AddTo(ViewportRenderer ^ viewport);
		virtual void RemoveFrom(ViewportRenderer ^ viewport);
		virtual event System::Action ^ Invalidated;
	private:
		SlabAxesVisualPrivates* privates;	
		Entities::Space^ space;
        Plane hiddenPlane;
		bool isHighlighted;
	};
}
