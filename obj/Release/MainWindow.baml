   M S B A M L   `   `   `  ���� A  =Grafos, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:GrafosGrafos      Grafos.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:Grafos     -     � Title$  Grafos�� � WindowStartupLocation$ CenterScreen=�$
��1096��$��696.5�� � 
ResizeMode$ NoResize=�
�������.��� -    �Name$
 grid��$��	0,84,6,-2q� � ColumnDefinitions �� $
��213* ��� $
��329* �.+�� -   $ header��$
��Left=�$��80��$	��Top=�$
��1094��$��-6,-90,-4,0q�$��2��.+��� ���	#FF1E1E5D$
��Left=�$��4��$��	10,54,0,0q�
���   �$	��Top=�$
��1084���� $L��FIntegrantes: Marco Rodrigo Flores Tepatzi - Jorge Jasiel Salmeron Vega��$
��Left=�$��9,18,0,0q�$	��Top=�$	��858��
���   ���Z� � -   $ 	controles��$
��Left=�$	��583��$��384,-35,0,0q�$	��Top=�$	��272��$��1��.+��� $��	Controles��$
��Left=�$	��346��$��0,10,0,0q�$	��Top=�$	��252��
���   ����	#FF009FFF.��� $
��Left=�$	��313��$��
10,10,-2,0q�$	��Top=�$	��232��.+��� -    ��Name$ 	lv_matriz��$
��Left=�$	��122��$��	10,82,0,0q�$	��Top=�$	��212��$ �� IsSynchronizedWithCurrentItem$
 TrueY�$��10(����	#FF009FFF
���   ����� *_item+��Z�  �� View �� �� -    ��Name$ btnPuntoPartida��   RoundButtonTemplate#��[  $��Punto partida��$
��Left=�$��90,5,0,0q�$	��Top=�$��86��$��25��$��3q�$y�Hidden=��� -   $ btnNuevoNodo��#��[  $��Añadir nodo��$
��Left=�$��10,5,0,0q�$	��Top=�$��1��$��25��$��75��$��3q��� -   $ btnAnteriorPeso��#��[  $��<��$
��Left=�$��	42,76,0,0q�$	��Top=�$��24��{�.  $��22��$y�Hidden=��� -	   $ 	btnInicio��#��[  $��Inicio��$
��Left=�$	��Top=�$��75��$��	10,35,0,0q�$��25��$��3q��� -
   $ btnSiguientePeso��#��[  $��>��$
��Left=�$��
176,78,0,0q�$	��Top=�$��24��{�.  $��22��$y�Hidden=��� -   $ btnGuardarPeso��$��Guardar peso��#��[  $
��Left=�$��
10,114,0,0q�$	��Top=�$��75��$��25��$��3q�$y�Hidden=��� -   $ btnArbol��#��[  $��	Ver arbol��$
��Left=�$��	90,35,0,0q�$	��Top=�$��86��$��25��{�.  $��3q�$y�Hidden=��� -   $ btnCrearArista��#��[  $��Crear arista��$
��Left=�$��90,5,0,0q�$	��Top=�$��86��$��25��$��3q��� -   	 ��Name$	 ar_label��$	��0-0��$
��Left=�$��	71,74,0,0q�$	��Top=�
���   �$y�Hidden=��� -   $	 nombre_nodo��$�� ��$
��Left=�$��
10,139,0,0q�$	��Top=�$��26��$��10���� -   $	 nodo_inicial_lb��$��Nodo inicial: 0��$
��Left=�$��	5,139,0,0q�$	��Top=�
 J� RenderTransformOrigin$
 0.116,0.577&�$��26��$��86��
���   �$y�Hidden=��� -   $	 label_1��$
��Left=�$��111,139,0,0q�$	��Top=�$��26��$	��102�����	#FF000104$y�Hidden=��� $��Peso:��$
��Left=�$��5,72,0,0q�$	��Top=�
���   �$y�Hidden=��� -    ��Name$ tbPeso��$
��Left=�$��22��$��
104,78,0,0q� �� TextWrapping$
 Wrap=�$��0��$	��Top=�$��67��
�������
���   �$y�Hidden=��� -   $ btnExpandir��#��[  $��Expandir��$
��Left=�$��	90,35,0,0q�$	��Top=�$��86��$��25��$��3q��� -    ��Name$ 	salida_gb��$��Salida��$
��Left=�$	��212��$��	0,361,0,0q�$	��Top=�$	��252��
���   ����	#FF009FFF.���� -    ��Name$ salida_text��$
��Left=�$��31,10,-2,-4q�$
��Wrap=�$	��Top=�$	��183��$	��211���� -    ��Name$ canvas��$
��Left=�$	��567��$��
10,-24,0,0q�$	��Top=�$	��792��$��2���� -   $	 nombre_arista��$�� ��$
��Left=�$��0,38,0,0q�$	��Top=�$��26��$��10���� -   $	 coordenadas��$�� ��$
��Left=�$��292,545,0,0q�$	��Top=�$��26��$��84��$��1���� -   $		 msg��$�� ��$
��Left=�$��
10,549,0,0q�$	��Top=�$��23��$	��703��$��2��
���   ��� -   $	 label_2��$�� ��$
��Left=�$��
336,87,0,0q�$	��Top=�$��26��$��10��