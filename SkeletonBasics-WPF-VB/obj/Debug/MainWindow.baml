   M S B A M L   `   `   `  ����  SkeletonBasics-WPF8    2Microsoft.Samples.Kinect.SkeletonBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Grupo 10 GPU y FPU��5   	   $��640.2��6$   $��1524.8��63   $��Center=�6p   $��Center=�6�   $��Center=�6�   $��Center=�6�   
���   � � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SquareCheckBox �� 
TargetType �� Orientation �Name	 ��Name
 ��Name �� TextAlignment �� Triggers P� Property P� Value �� 
TargetName��%1  &	        &	 #     &	 F     '�� i     &	 �     7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5	   
   $ 	#ff00BCF2��62   �� 5
   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
Horizontal=�6)   
������ .+�5   "   � $ SquareCheckBoxChecked��6'   .+�5   &   �� $	 CheckedNormal��6,   $��Images\CheckedNormal.png��6C   $
��None=�6e   $��Center=�6t   �� 5   &   $	 CheckedHover��6,   $��Images\CheckedHover.png��6B   $
��None=�6c   $��Center=�6r   $y�	Collapsed=�6�   5   #   � 5   "   $ SquareCheckBoxUnchecked��6'   $y�	Collapsed=�6H   .+�5   &   �� $	 UncheckedNormal��6,   $ ��Images\UncheckedNormal.png��6E   $
��None=�6i   $��Center=�6x   �� 5   &   $	 UncheckedHover��6,   $��Images\UncheckedHover.png��6D   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   5   #   �� 5   "   $
 SquareCheckBoxText��6,   #��z��6H   $
 Left=�6i   $��Center=�6   8��  $��15(�6�   $��9,0,0,0q�6�   5      5       5      P� 5       �@d�	IsChecked$ falseY�6;   . �5    "   �� $ SquareCheckBoxChecked��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5!   "   $ SquareCheckBoxUnchecked��6O    � y�$ Visible=�6?   6O   5"      P� 5#       �@J�IsMouseOver . . �5$   "   �� $ CheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5%   "   $ UncheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5&   "   $ CheckedHover��6O    � y�$ Visible=�6?   6O   �� 5'   "   $ UncheckedHover��6O    � y�$ Visible=�6?   6O   �� 5(   "   8  $ SquareCheckBoxText��6h    � ��5)      5+      5-      5.      5/      .��51      � -   5        $��
layoutGrid��51      $��10,0,10.4,0q�6   $	��607��62   $	��Top=�6?    � RowDefinitions 52   
   �� 53      $
��Auto �6   �� 54      $��* �6   �� 55      $
��Auto �6   �� 56      $
��Auto �6   .+�58   
   ]� $��0��6   $��0 0 0 20q�6!   .+�59      �� $��Right=�6   $��0 0 -1 0q�6/   $��Bottom=�6A   ���	#FFC7C7C7$��Segoe UI)�6s   $��18(�6�   $��Center=�6�   Arquitectura de Computadores5:      5;      (� 5<   
   $��1��6    (� Stretch$ Uniform=�6   $
��Left=�61   .��5=      �� -   5        $��Image��5=      $	��480��6!   $
��Left=�6.   $	��640��6I    J� SnapsToDevicePixels . 6U   5>      �� 5?   
   -   5        $��	statusBar��5?   ?   $��3��6   $��Stretch=�6!   $	��Top=�6P   
�������#��[  6{   $��	0,0.4,0,0q�6�   .@�5@      �� $��0 0 0 10q�6    J� RenderTransformOrigin$ 0.502,-2.117&�6/   $��Bottom=�6T   $��Center=�6v   ���	#FF313131$��Bottom=�6�    � IsSharedSizeScope .  . .��5A      �� -   5        $��statusBarText��5A      $��-1 0 0 0q�61   ...6H   5B      5C      �� 5D   
   -   5         ��Name$ 	CheckBox1��5D      $��Points��6&   $
��Left=�67   $��	406,5,0,0q�6R   $��3��6e   $	��Top=�6r   �� 5E   
   -   5        $ 	CheckBox2��5E      $��Lines��6&   $
��Left=�66   $��	534,5,0,0q�6Q   $��3��6d   $	��Top=�6q   �� 5F   
   -   5        $ 	CheckBox3��5F      $��
True Image��6&   $
��Left=�6;   $��	680,5,0,0q�6V   $��3��6i   $	��Top=�6v   �� 5G   
   -	   5        $	 Image2��5G      $
��Left=�6    $��
0,0,0,25.2q�6;   $	��717��6O   $y�Hidden=�6[   $��1��6o   $��3��6|   r� 5H   
   -
   5         r�Name$ Video1��5H      $��Right=�6'   $��
0,0,0,12.2q�6C   $��1��6W   $	��696��6d    r� Source$ Images/Sin títulowmv.wmv?�6p    r� UnloadedBehavior$ Manual=�6�   $��3��6�    r� Volume$	 100Q�6�    r� 
SpeedRatio$ 1Q�6�    r� LoadedBehavior$ Manual=�6�   �� 5I   
   -   5         ��Name$	 BTN��5I      $��Button��6   $
��Left=�6/   $��
722,45,0,0q�6J   $��1��6^   $	��Top=�6k   $��75��6�   6�   5K      5L      