   M S B A M L   `   `   `  ����I  ERestaurant_SAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Restaurant_SAP.ViewsRestaurant_SAP.Views  5      E'clr-namespace:Restaurant_SAP.PropertiesRestaurant_SAP.Properties  5      $    Restaurant_SAP.Views.MenusView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Restaurant_SAP.Views     5      5p'clr-namespace:Restaurant_SAP.Properties     5      .��5
      �   � RowDefinitions  5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��10q�6c   ��6   �� *Menus+ �� SelectedItem 6=   �� *SelectedMenu+��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $
��Bold"�6<   $��0,0,5,0q�6N   )   #Restaurant_SAP.Properties.Resources   ListBox_Menus_Nombre � ToolTip# Z 6_   ��6$   �� *Nombre+6_   �� 5      $��0,0,5,0q�6A       ListBox_Menus_Descripcion# Z 6R   ��6$   �� *Descripcion+6R   �� 5         ListBox_Menus_Precio# Z 6<   ��6$   �� *Precio+6<   5      5      5      �� 5   
   $��1��6      GroupBox_EdicionMenu#��Z 6    $��10q�6U    � DataContext 6a   �� *MenuEnEdicion+.��5      � 	 � ColumnDefinitions	 5      �� 5       $
��Auto �6'   �� 5!      $��* �6'     5#      �� 5$      $
��Auto �6$   �� 5%      $
��Auto �6$   �� 5&      $
��Auto �6$   �� 5'      $
��Auto �6$   .+�5*      �� $��0��6   $��0��6)   
   TextBlock_NombreMenu#��Z
 69   $��5q�6l   �� 5+      $��0��6   $��1��6'   $��5q�6t   ��67   �� *Nombre+ �� UpdateSourceTrigger$ PropertyChanged=�6t   �� 5-      $��1��6   $��0��6)       TextBlock_DescripcionMenu#��Z 69   $��5q�6q   �� 5.      $��1��6   $��1��6'   $��5q�6y   ��67   �� *Descripcion+$ PropertyChanged=�6y   �� 50      $��2��6   $��0��6)      TextBlock_PrecioMenu#��Z 69   $��5q�6l   �� 51      $��2��6   $��1��6'   $��5q�6t   ��67   �� *Precio+$ PropertyChanged=�6t   �� 53      $��3��6   $��2��6*   $ 
Horizontal=�6>   $��Right=�6W   .+�54      ��    Button_GuardarMenu#��Z 6   $��5q�6�   ��6Q   �� *#!DataContext.GuardarCambiosCommand+ �� RelativeSource ��  �� AncestorType UserControl6�   �� 55         Button_CancelarMenu#��Z 6   $��5q�6�   ��6R   �� *$"DataContext.CancelarEdicionCommand+ ��  UserControl6�   �� 56         Button_EliminarMenu#��Z 6   $��5q�6�   ��6R   �� *!DataContext.EliminarMenuCommand+ ��  UserControl6�   57      58      59      �� 5;   
   $��2��6      GroupBox_NuevoMenu#��Z 6    $��10q�6S    6_   �� *	NuevoMenu+.��5<      � 	 5=      �� 5>      $
��Auto �6'   �� 5?      $��* �6'     5A      �� 5B      $
��Auto �6$   �� 5C      $
��Auto �6$   �� 5D      $
��Auto �6$   �� 5E      $
��Auto �6$   .+�5H      �� $��0��6   $��0��6)   #��Z
 69   $��5q�6l   �� 5I      $��0��6   $��1��6'   $��5q�6t   ��67   �� *Nombre+$ PropertyChanged=�6t   �� 5K      $��1��6   $��0��6)   #��Z 69   $��5q�6q   �� 5L      $��1��6   $��1��6'   $��5q�6y   ��67   �� *Descripcion+$ PropertyChanged=�6y   �� 5N      $��2��6   $��0��6)   #��Z 69   $��5q�6l   �� 5O      $��2��6   $��1��6'   $��5q�6t   ��67   �� *Precio+$ PropertyChanged=�6t   �� 5Q      $��3��6   $��2��6&      Button_AgregarNuevoMenu#��Z 6:   $��5q�5R   �   ��6   �� * DataContext.AgregarMenuCommand+ ��  UserControl6�   5S      5T      5U      5V      