   M S B A M L   `   `   `  ����I  ERestaurant_SAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Restaurant_SAP.ViewsRestaurant_SAP.Views  5      E'clr-namespace:Restaurant_SAP.PropertiesRestaurant_SAP.Properties  5      1    +Restaurant_SAP.Views.HistorialEconomicoView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Restaurant_SAP.Views     5      5p'clr-namespace:Restaurant_SAP.Properties     5      -   5        .��5      �   � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��10q�6;   .+�5      �� )   #Restaurant_SAP.Properties.Resources   TextBlock_FiltrarPorMesa#��Z 6   $��5,0,5,0q�6O   $��Center=�6`   �� 5      -   5         ��Name$ cmbFiltrarMesa��5      $	��100��6/   $��0,0,10,0q�6;    �� DisplayMemberPath$ Numero��6k   ��6M   �� *Mesas+ �� SelectedItem 6�   �� *MesaSeleccionada+�� 5         TextBlock_FiltrarPorMenu#��Z 6   $��5,0,5,0q�6O   $��Center=�6`   �� 5      -   5        $ cmbFiltrarMenu��5      $	��150��6/   $��0,0,10,0q�6;   $ Nombre��6k   ��6M   �� *Menus+ 6�   �� *MenuSeleccionado+�� 5         TextBlock_DesdeFecha#��Z 6   $��5,0,5,0q�6K   $��Center=�6\   (  "System.Windows.Controls.DatePicker  5      -   5          Name$ dpDesde��5      $��0,0,10,0q�6*   	   SelectedDate	 6<   �� *
FechaDesde+
 �� UpdateSourceTrigger$
 PropertyChanged=��� 5         TextBlock_HastaFecha#��Z 6   $��5,0,5,0q�6K   $��Center=�6\     5      -   5        $ dpHasta��5      	 6*   �� *
FechaHasta+$
 PropertyChanged=��� 5!         Button_BuscarPedidos#��Z 6   $��10,0,0,0q�6K   ��6]   �� *BuscarPedidosCommand+5"      �� 5$   
   $��1��6   $��10q�6@   $��Stretch=�6L   ��6    �� *	Pedidos+ �� View 5%      �� 5&      .P�5'      ��    GridViewColumn_FechaHora#��Z 6%    �� Width$
 Auto��6�    �� DisplayMemberBinding 6^   �� *	FechaHora+ �� StringFormat$ dd/MM/yyyy HH:mm��6�   �� 5(          GridViewColumn_NombreMenu#��Z 6%   $
 Auto��6�    6_   �� *Menu.Nombre+6�   �� 5)      %   GridViewColumn_DescripcionMenu#��Z 6%   $	 400��6�    6d   �� *Menu.Descripcion+6�   �� 5*      )   "GridViewColumn_NumeroMesaHistorial#��Z 6%   $
 Auto��6�    6h   �� *Mesa.Numero+6�   �� 5+      %   GridViewColumn_PrecioHistorial#��Z 6%   $
 Auto��6�    6d   �� *Precio+$ C��6�   5,      ��5.      �� 5/       �� 
TargetType" ��6   .�50      ��  �� Property � �� �� Value$ Stretch=�6C   �� 51       � ��$ 2q�6/   52      54      �� 56   
   $��2��6   
�������$��1,0,0,0q�66    �� Padding$ 5q�6P   ���#EEEEEE._�57      �  � ColumnDefinitions 58      �� 59      $��* �6'   �� 5:      $
��Auto �6'   .+�5=      �� $��0��6      TextBlock_Total#��Z 6,   $��Right=�6Z   $
��Bold"�6v   $��5q�6�   �� 5>      $��1��6   $��Right=�6S   $��5q�6o   $
��Bold"�6z   ��6,   �� *Total+$ C��6z   5?      5@      �� 5B   
   $��3��6      Button_GenerarPDF#��Z 6   $��10q�6Q   $��Right=�6�   $	��120��6�   ��6]   �� *GenerarPdfCommand+6�   5C      5D      