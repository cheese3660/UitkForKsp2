﻿using UnityEngine;
using UnityEngine.UIElements;

namespace Ksp2Uitk.API;

public class DragManipulator: IManipulator
{
    private VisualElement _target;
    private Vector3 _offset;
    private PickingMode _mode;

    public bool Dragging { get; set; }

    public VisualElement target
    {
        get => _target;
        set
        {
            _target = value;

            _target.RegisterCallback<PointerDownEvent>(DragBegin);
            _target.RegisterCallback<PointerUpEvent>(DragEnd);
            _target.RegisterCallback<PointerMoveEvent>(PointerMove);
        }
    }

    private void DragBegin(PointerDownEvent evt)
    {
        _mode = target.pickingMode;
        target.pickingMode = PickingMode.Ignore;
        _offset = evt.localPosition;
        Dragging = true;
        target.CapturePointer(evt.pointerId);
    }

    private void DragEnd(IPointerEvent evt)
    {
        target.ReleasePointer(evt.pointerId);
        Dragging = false;
        target.pickingMode = _mode;
    }

    private void PointerMove(PointerMoveEvent evt)
    {
        if (!Dragging)
        {
            return;
        }
        var delta = evt.localPosition - _offset;
        target.transform.position += delta;
    }
}