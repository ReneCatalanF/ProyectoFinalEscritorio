   M S B A M L   `   `   `  ����I  ERestaurant_SAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Restaurant_SAP.ViewsRestaurant_SAP.Views  5      E'clr-namespace:Restaurant_SAP.ViewModelsRestaurant_SAP.ViewModels  5      E'clr-namespace:Restaurant_SAP.ConvertersRestaurant_SAP.Converters  5      =#clr-namespace:Restaurant_SAP.ModelsRestaurant_SAP.Models  5	      E'clr-namespace:Restaurant_SAP.PropertiesRestaurant_SAP.Properties  5
      $    Restaurant_SAP.Views.MesasView   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Restaurant_SAP.Views     5      >
viewmodels'clr-namespace:Restaurant_SAP.ViewModels     5      >
converters'clr-namespace:Restaurant_SAP.Converters     5      6models#clr-namespace:Restaurant_SAP.Models     5	      5p'clr-namespace:Restaurant_SAP.Properties     5
      .��5      �   � RowDefinitions  5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��10q�6c   ��6   �� *Mesas+ �� SelectedItem 6=   �� *SelectedMesa+��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $
��Bold"�6<   $��0,0,5,0q�6N   )   #Restaurant_SAP.Properties.Resources   ListBox_Mesas_Numero � ToolTip# Z 6_   ��6$   �� *Numero+6_   �� 5      $��0,0,5,0q�6A       ListBox_Mesas_Descripcion# Z 6R   ��6$   �� *Descripcion+6R   �� 5         ListBox_Mesas_Estado# Z 6<   ��6$   �� *Estado+6<   5      5      5      �� 5    
   $��1��6      GroupBox_EdicionMesa#��Z 6    $��10q�6U    � DataContext 6a   �� *MesaEnEdicion+.��5!      � 	 � ColumnDefinitions	 5"      �� 5#      $
��Auto �6'   �� 5$      $��* �6'     5&      �� 5'      $
��Auto �6$   �� 5(      $
��Auto �6$   �� 5)      $
��Auto �6$   �� 5*      $
��Auto �6$   .+�5-      �� $��0��6   $��0��6)   
   TextBlock_NumeroMesa#��Z
 69   $��5q�6l   �� 5.      $��0��6   $��1��6'   $��5q�6t   ��67   �� *Numero+ �� UpdateSourceTrigger$ PropertyChanged=�6t   �� 50      $��1��6   $��0��6)       TextBlock_DescripcionMesa#��Z 69   $��5q�6q   �� 51      $��1��6   $��1��6'   $��5q�6y   ��67   �� *Descripcion+$ PropertyChanged=�6y   �� 53      $��2��6   $��0��6)      TextBlock_EstadoMesa#��Z 69   $��5q�6l   �� 54      $��2��6   $��1��6(   $��5q�56   `   ��55      �� *DataContext.EstadosMesa+ �� RelativeSource ��  �� AncestorType UserControl 56      �� *Estado+$ PropertyChanged=�6`   �� 58      $��3��6   $��2��6*   $ 
Horizontal=�6>   $��Right=�6W   .+�59      ��    Button_GuardarMesa#��Z 6   $��5q�6�   ��6Q   �� *#!DataContext.GuardarCambiosCommand+ ��  UserControl6�   �� 5:         Button_CancelarMesa#��Z 6   $��5q�6�   ��6R   �� *$"DataContext.CancelarEdicionCommand+ ��  UserControl6�   �� 5;         Button_EliminarMesa#��Z 6   $��5q�6�   ��6R   �� *!DataContext.EliminarMesaCommand+ ��  UserControl6�   5<      5=      5>      �� 5@   
   $��2��6      GroupBox_NuevaMesa#��Z 6    $��10q�6S    6_   �� *	NuevaMesa+.��5A      � 	 5B      �� 5C      $
��Auto �6'   �� 5D      $��* �6'     5F      �� 5G      $
��Auto �6$   �� 5H      $
��Auto �6$   �� 5I      $
��Auto �6$   .+�5L      �� $��0��6   $��0��6)   #��Z
 69   $��5q�6l   �� 5M      $��0��6   $��1��6'   $��5q�6t   ��67   �� *Numero+$ PropertyChanged=�6t   �� 5O      $��1��6   $��0��6)   #��Z 69   $��5q�6q   �� 5P      $��1��6   $��1��6'   $��5q�6y   ��67   �� *Descripcion+$ PropertyChanged=�6y   �� 5R      $��2��6   $��2��6&      Button_AgregarNuevaMesa#��Z 6:   $��5q�5S   �   ��6   �� * DataContext.AgregarMesaCommand+ ��  UserControl6�   5T      5U      5V      5W      