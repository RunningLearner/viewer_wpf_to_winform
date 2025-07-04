#pragma once

#include "ViewportRenderer.h"

namespace RenderEngine2 {

	public interface class IVisual
	{
		void PreRender(ViewportRenderer^ viewport);
		virtual void AddTo(ViewportRenderer^ viewport);
		virtual void RemoveFrom(ViewportRenderer^ viewport);
		event System::Action^ Invalidated;
	};
}
