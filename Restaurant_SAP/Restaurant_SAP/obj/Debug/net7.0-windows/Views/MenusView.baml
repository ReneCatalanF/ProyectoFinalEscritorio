   M S B A M L   `   `   `  ����I  ERestaurant_SAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Restaurant_SAP.ViewsRestaurant_SAP.Views  5      $    Restaurant_SAP.Views.MenusView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Restaurant_SAP.Views     5      .��5	      �   � RowDefinitions  5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��10q�6c   ��6   �� *Menus+ �� SelectedItem 6=   �� *SelectedMenu+��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $
��Bold"�6<   $��0,0,5,0q�6N   ��6$   �� *Nombre+6N   �� 5      $��0,0,5,0q�6A   ��6$   �� *Descripcion+6A   �� 5      ��6$   �� *Precio+5      5      5      �� 5   
   $��1��6   $��Edición de Menu��6    $��10q�69    � DataContext 6E   �� *MenuEnEdicion+.��5      �  � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5       $��* �6'     5"      �� 5#      $
��Auto �6$   �� 5$      $
��Auto �6$   �� 5%      $
��Auto �6$   �� 5&      $
��Auto �6$   .+�5)      �� $��0��6   $��0��6)   $��Nombre:��69   $��5q�6H   �� 5*      $��0��6   $��1��6'   $��5q�6t   ��67   �� *Nombre+ �� UpdateSourceTrigger$ PropertyChanged=�6t   �� 5,      $��1��6   $��0��6)   $��Descripción:��69   $��5q�6M   �� 5-      $��1��6   $��1��6'   $��5q�6y   ��67   �� *Descripcion+$ PropertyChanged=�6y   �� 5/      $��2��6   $��0��6)   $��Precio:��69   $��5q�6H   �� 50      $��2��6   $��1��6'   $��5q�6t   ��67   �� *Precio+$ PropertyChanged=�6t   �� 52      $��3��6   $��2��6*   $ 
Horizontal=�6>   $��Right=�6W   .+�53      �� $��Guardar��6   $��5q�6�   ��6/   �� *#!DataContext.GuardarCambiosCommand+ �� RelativeSource ��  �� AncestorType UserControl6�   �� 54      $��Cancelar��6   $��5q�6�   ��60   �� *$"DataContext.CancelarEdicionCommand+ ��  UserControl6�   �� 55      $��Eliminar��6   $��5q�6�   ��60   �� *!DataContext.EliminarMenuCommand+ ��  UserControl6�   56      57      58      �� 5:   
   $��2��6   $��
Nueva Menu��6    $��10q�64    6@   �� *	NuevoMenu+.��5;      �  5<      �� 5=      $
��Auto �6'   �� 5>      $��* �6'     5@      �� 5A      $
��Auto �6$   �� 5B      $
��Auto �6$   �� 5C      $
��Auto �6$   �� 5D      $
��Auto �6$   .+�5G      �� $��0��6   $��0��6)   $��Nombre:��69   $��5q�6H   �� 5H      $��0��6   $��1��6'   $��5q�6t   ��67   �� *Nombre+$ PropertyChanged=�6t   �� 5J      $��1��6   $��0��6)   $��Descripción:��69   $��5q�6M   �� 5K      $��1��6   $��1��6'   $��5q�6y   ��67   �� *Descripcion+$ PropertyChanged=�6y   �� 5M      $��2��6   $��0��6)   $��Precio:��69   $��5q�6H   �� 5N      $��2��6   $��1��6'   $��5q�6t   ��67   �� *Precio+$ PropertyChanged=�6t   �� 5P      $��3��6   $��2��6&   $��Agregar Menu��6:   $��5q�5Q   ~   ��6   �� * DataContext.AgregarMenuCommand+ ��  UserControl6~   5R      5S      5T      5U      