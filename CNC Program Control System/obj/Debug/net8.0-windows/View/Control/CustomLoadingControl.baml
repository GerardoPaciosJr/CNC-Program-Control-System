   M S B A M L   `   `   `  ����U  QCNC Program Control System, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:CNC_Program_Control_SystemCNC_Program_Control_System  5      5    /CNC_Program_Control_System.CustomLoadingControl   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:CNC_Program_Control_System     5        � 	Resources  5         ElasticLines g� RepeatBehavior g� AutoReverse �� TargetProperty �� 
TargetName8  2System.Windows.Media.Animation.EasingPointKeyFrame $� KeyTime $� Value��%�   &	        �� 5   
   $ Forever��6*   $
 True��6C   .��5      *� $1 +(Shape.Fill).(LinearGradientBrush.EndPoint)�6+   $ 
ElasticBox��6s   .'�5        $ 0:0:4��6&   $
 12,8&�66   5      5      5       � Triggers 5      :� 5   
    :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5      �� #��[  6   5      .��5   
   � $��Center=�6   $��Center=�6,   .+�5-   
   �� $&�� /Images/eXcellerant_mfg_icon.jpg��6   �� 5.   
   .+�51      � .+�52      �� -   5        	 ��Name$	 
ElasticBox��52       
�������$��10��53   &   $��20q�6;   ��54      �� 55      
 �� 
StartPoint$	
 0,0&�62    �� EndPoint$	 6,4&�6C    � MappingMode$ Absolute=�6R    � SpreadMethod$ Repeat=�6i   .��56      �  � Color$ White��6+    � Offset$
 0.25Q�69   � 57   "   $ 	#00000000��6/   $
 0.15Q�6A   58      5:      �� 5<      5>      5B      5D      5F      