using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Voxell.Rasa;

namespace Voxell.UI
{
  public class RasaNodeView : Node
  {
    public Action<RasaNodeView> OnNodeSelected;
    public Action OnNodeUnSelected;
    public RasaNode rasaNode;
    public List<Port> inputPorts;
    public List<Port> outputPorts;

    public RasaNodeView(RasaNode rasaNode)
    {
      this.rasaNode = rasaNode;
      this.title = rasaNode.name;
      this.viewDataKey = rasaNode.guid;

      style.left = rasaNode.position.x;
      style.top = rasaNode.position.y;

      inputPorts = new List<Port>();
      outputPorts = new List<Port>();

      List<PortInfo> inputPortInfos = rasaNode.CreateInputPorts();
      List<PortInfo> outputPortInfos = rasaNode.CreateOutputPorts();

      for (int ip=0; ip < inputPortInfos.Count; ip++)
      {
        Port port = InstantiatePort(
          Orientation.Horizontal,
          Direction.Input,
          (Port.Capacity)inputPortInfos[ip].capacityInfo,
          inputPortInfos[ip].portType
        );
        port.portColor = inputPortInfos[ip].color;
        port.portName = inputPortInfos[ip].portName;
        inputPorts.Add(port);
        inputContainer.Add(port);
      }

      for (int op=0; op < outputPortInfos.Count; op++)
      {
        Port port = InstantiatePort(
          Orientation.Horizontal,
          Direction.Output,
          (Port.Capacity)outputPortInfos[op].capacityInfo,
          outputPortInfos[op].portType
        );
        port.portColor = outputPortInfos[op].color;
        port.portName = outputPortInfos[op].portName;
        outputPorts.Add(port);
        outputContainer.Add(port);
      }
    }

    public override void SetPosition(Rect newPos)
    {
      base.SetPosition(newPos);
      Undo.RecordObject(rasaNode, "Rasa Tree (Set Node Position)");
      EditorUtility.SetDirty(rasaNode);
      rasaNode.position.x = newPos.xMin;
      rasaNode.position.y = newPos.yMin;
    }

    public override void OnSelected()
    {
      base.OnSelected();
      OnNodeSelected?.Invoke(this);
    }

    public override void OnUnselected()
    {
      base.OnUnselected();
      OnNodeUnSelected?.Invoke();
    }
  }
}