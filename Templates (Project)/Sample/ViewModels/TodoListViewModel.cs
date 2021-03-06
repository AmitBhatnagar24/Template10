﻿using System;
using System.Collections.ObjectModel;
using Template10.Mvvm;

namespace Template10.ViewModels
{
    public class TodoListViewModel : Mvvm.ViewModelBase
    {
        Repositories.TodoItemRepository _todoItemRepository;

        public TodoListViewModel(Models.TodoList list)
        {
            _todoItemRepository = new Repositories.TodoItemRepository();

            this.TodoList = list;
            foreach (var item in list.Items)
            {
                this.Items.Add(new ViewModels.TodoItemViewModel(item));
            }
        }

        private Models.TodoList _TodoList = default(Models.TodoList);
        public Models.TodoList TodoList { get { return _TodoList; } set { Set(ref _TodoList, value); } }

        private ObservableCollection<ViewModels.TodoItemViewModel> _Items = new ObservableCollection<TodoItemViewModel>();
        public ObservableCollection<ViewModels.TodoItemViewModel> Items { get { return _Items; } private set { Set(ref _Items, value); } }

        private ViewModels.TodoItemViewModel _SelectedItem = default(ViewModels.TodoItemViewModel);
        public ViewModels.TodoItemViewModel SelectedItem { get { return _SelectedItem; } set { Set(ref _SelectedItem, value); SelectedItemIsSelected = (value != null); } }

        private bool _SelectedItemIsSelected = default(bool);
        public bool SelectedItemIsSelected { get { return _SelectedItemIsSelected; } set { Set(ref _SelectedItemIsSelected, value); } }

        DelegateCommand<string> _AddCommand = default(DelegateCommand<string>);
        public DelegateCommand<string> AddCommand { get { return _AddCommand ?? (_AddCommand = new DelegateCommand<string>(ExecuteAddCommand, CanExecuteAddCommand)); } }
        private bool CanExecuteAddCommand(string title) { return true; }
        private void ExecuteAddCommand(string title)
        {
            try
            {
                var index = this.Items.IndexOf(this.SelectedItem);
                var item = new ViewModels.TodoItemViewModel(_todoItemRepository.Factory(title: title));
                this.TodoList.Items.Insert((index > -1) ? index : 0, item.TodoItem);
                this.Items.Insert((index > -1) ? index : 0, item);
                this.SelectedItem = item;
            }
            catch { this.SelectedItem = null; }
        }

        DelegateCommand<Models.TodoItem> _RemoveCommand = default(DelegateCommand<Models.TodoItem>);
        public DelegateCommand<Models.TodoItem> RemoveCommand { get { return _RemoveCommand ?? (_RemoveCommand = new DelegateCommand<Models.TodoItem>(ExecuteRemoveCommand, CanExecuteRemoveCommand)); } }
        private bool CanExecuteRemoveCommand(Models.TodoItem param) { return this.SelectedItem != null; }
        private void ExecuteRemoveCommand(Models.TodoItem param)
        {
            try
            {
                var index = this.Items.IndexOf(this.SelectedItem);
                this.TodoList.Items.Remove(this.SelectedItem.TodoItem);
                this.Items.Remove(this.SelectedItem);
                this.SelectedItem = this.Items[index];
            }
            catch { this.SelectedItem = null; }
        }
    }
}
