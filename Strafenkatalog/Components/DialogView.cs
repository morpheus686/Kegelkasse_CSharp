﻿using System.Windows;

namespace Strafenkatalog.Components
{
    public class DialogView : LoadableView
    {
        public static readonly DependencyProperty DialogViewTypeProperty =
    DependencyProperty.Register(
        "DialogViewType",
        typeof(DialogViewType),
        typeof(DialogView),
        new PropertyMetadata(DialogViewType.None));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(string),
                typeof(DialogView),
                new PropertyMetadata(String.Empty));

        public DialogViewType DialogViewType
        {
            get { return (DialogViewType)GetValue(DialogViewTypeProperty); }
            set { SetValue(DialogViewTypeProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}
