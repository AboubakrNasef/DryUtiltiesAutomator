using DUA_WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DUA_WPF.Services
{
    public class ModalService : IModalService
    {


        public ViewModelBase ModalContent { get; private set; }
        public bool IsModalOpen { get; private set; }

        public event Action ModalStateChanged;

        public void CloseModal()
        {
          ModalContent = null;
          IsModalOpen = false;
            ModalStateChanged?.Invoke();
        }

        public void OpenModal(ViewModelBase content)
        {
            ModalContent = content;
            IsModalOpen = true;
            ModalStateChanged?.Invoke();
        }

        public ModalService()
        {
            ModalContent = null;
            IsModalOpen = false;
           
        }
    }

    public interface IModalService
    {
        void OpenModal(ViewModelBase content);

        void CloseModal();
        bool IsModalOpen { get; }
        ViewModelBase ModalContent { get; }

        event Action ModalStateChanged;
    }
}
