using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Metaproject.UndoRedo
{
    public class UndoRedoManager
    {


        Stack<IOperation> _undoStack = new Stack<IOperation>();
        Stack<IOperation> _redoStack = new Stack<IOperation>();


        public void Do(IOperation operation)
        {
            operation.Do();
            _undoStack.Push(operation);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (!IsAnythingToUndo) return;

            IOperation operation = _undoStack.Pop();
            operation.Undo();
            _redoStack.Push(operation);
        }


        public void Redo()
        {

            if (!IsAnythingToRedo) return;

            IOperation operation = _redoStack.Pop();
            operation.Redo();
            _undoStack.Push(operation);
        }

        public bool IsAnythingToUndo
        {
            get
            {
                return _undoStack.Count > 0;
            }
        }

        public bool IsAnythingToRedo
        {
            get
            {
                return _redoStack.Count > 0;
            }

        }








        public void Clear()
        {
            _redoStack.Clear();
            _undoStack.Clear();
        }
    }
}
