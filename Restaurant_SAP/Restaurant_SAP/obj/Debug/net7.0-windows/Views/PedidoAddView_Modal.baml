   M S B A M L   `   `   `  ����I  ERestaurant_SAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:Restaurant_SAP.ViewsRestaurant_SAP.Views  5      E'clr-namespace:Restaurant_SAP.PropertiesRestaurant_SAP.Properties  5      .    (Restaurant_SAP.Views.PedidoAddView_Modal   5      W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:Restaurant_SAP.Views     5      5p'clr-namespace:Restaurant_SAP.Properties     5      .��5
      �   � RowDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6"   .+�5      �� )   #Restaurant_SAP.Properties.Resources    TextBlock_AgregandoPedido#��Z 6   $��5q�6P   �� 5      $�� ��6   $��5q�6    5      �� 5   
   $��1��6   $��0��6!   !   TextBlock_MesaSeleccionada#��Z 61   $��5q�6j   �� 5   
   $��1��6   $��1��6!   $��5q�6[   ��61   �� *SelectedMesa.Descripcion+6[   �� 5"   
   $��2��6   $��0��6!      TextBlock_Menu#��Z 61   $��5q�6^   �� 5#   
   $��2��6   $��1��6    $��5q�60    �� DisplayMemberPath$ Nombre��5%      ��5$      �� *Menus+ �� SelectedItem 5&      �� *SelectedMenu+ �� UpdateSourceTrigger$ PropertyChanged=��� 5)   
   $��3��6   $��2��6      Button_AceptarPedido#��Z 6.   $��5q�5*   :   $��Right=�6E   ��6   �� *AgregarPedidoCommand+6E   �� 5-   
   $��3��6   $��0��6!   	   TextBlock_Cantidad#��Z	 61   $��5q�6b   �� 5.   
   -   5        $��3��5.      $��1��6   $��5q�6/   ��50      �� *	cantidadF+$ PropertyChanged=��� 51   
   $��4��6   $��2��6   
   Button_CancelarPedido#��Z
 6.   $��5q�52      $��Bottom=�6F   ��6   �� *CancelarPedidoCommand+6F   53      54      