﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace MovieCatalog
{
    class Navigator
    {
        private static NavigationService NavigationService { get; } = (Application.Current.MainWindow as MainWindow).NavigationService;

        public static void Navigate(object page)
        {
            NavigationService.Navigate(page);
        }

        public static void Navigate(string path, object param = null)
        {
            NavigationService.Navigate(new Uri(path, UriKind.RelativeOrAbsolute), param);
        }

        public static void GoBack()
        {
            NavigationService.GoBack();
        }

        public static void GoForward()
        {
            NavigationService.GoForward();
        }
    }
}
