   M S B A M L   `   `   `  ����W  SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   TextBoxTemplateBase  �� 
TargetType �Name'   !System.Windows.VisualStateManager    VisualStateGroups%  System.Windows.VisualStateGroup   States   System.Windows.VisualState   
Storyboard g� 	BeginTime g� Duration W� From	 W� To	 �� TargetProperty
 �� 
TargetName8   2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime  ControlDisabledBackground �� Value  ControlDisabledBorder  ControlReadOnlyBackground  ControlReadOnlyBorder ��Name J� Opacity  ControlHoveredBorder �� Color  ControlHoveredBackground  ControlFocusedBorder  ControlFocusedBackground ��Name �� Triggers P� Property P� Value �� Property �� Value 	 ControlDisabledForeground 
 ControlReadOnlyForeground  TextBoxBaseStyle �� 
TargetType  ControlBackground  ControlForeground  ValidationTooltipTemplate  ControlBorder �� BasedOn�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %�  &	        7  7  7  7  7  7  7  7  7 	 7 
 &	 �	    7  7  7  7  7   '��     7  �� 5      "  ��62   .R�5   
   � $
 root��6   #��z��6   #��z��6?    5        5	      CommonStates��. 5
        
Normal��. 5      �� .��5      W�  00:00:00.0006.   $ 00:00:00.500D�6G    15   .    067   $	 Opacity�5   .   $
 hoverBorder��5   .   5      5        5      	MouseOver��. 5      �� .��5      W�  00:00:00.0006.   $ 00:00:00.500D�6G    05   .    167   $	 Opacity�5   .   $
 hoverBorder��5   .   5      5        5      Disabled��. 5      �� .��5      �� $
 normalBorder��6;   $1	 +(Border.Background).(SolidColorBrush.Color)�5   ;   .g�5   "     $ 0��66   8   6B   5      �� 5       $
 normalBorder��6;   $2	 ,(Border.BorderBrush).(SolidColorBrush.Color)�5!   ;   .g�5"   "     $ 0��66   8  6B   5#      5$      5%        5&      ReadOnly��. 5'      �� .��5(      �� $
 normalBorder��6;   $1	 +(Border.Background).(SolidColorBrush.Color)�5)   ;   .g�5*   "     $ 0��66   8  6B   5+      �� 5,      $
 normalBorder��6;   $2	 ,(Border.BorderBrush).(SolidColorBrush.Color)�5-   ;   .g�5.   "     $ 0��66   8  6B   5/      50      51      52        53      FocusStates��. 54        Focused��. 55      �� .��56      W�  00:00:00.0006.   $ 00:00:00.300D�6G    057   .    167   $	 Opacity�58   .   $
 focusBorder��59   .   5:      5;        5<      	Unfocused��. 5=      �� .��5>      W�  00:00:00.0006.   $ 00:00:00.700D�6G    15?   .    067   $	 Opacity�5@   .   $
 focusBorder��5A   .   5B      5C      5D      .+�5H      �� $ normalBorder��6   #��z��5I      #��z��5J      #��z��5K      �� 5N      $ hoverBorder��6   $ 0Q�6*   #��z��5O      ��5P      �� 5Q      8  6&   ��5S      �� 5T      8  6&   5V      �� 5Y      $ focusBorder��6   $ 0Q�6*   #��z��5Z      ��5[      �� 5\      8  6&   ��5^      �� 5_      8  6&   5a      �� 5d      $ PART_ContentHost��6   #��z��5e      $��0q�5f      $��False��5g      $��2q�5h      #��z��5j      #��z��5k      #��z��5l      ��5i      Z� 5l      5n       5o   
   P� 5p       � {�$ False��6+   . �5q      ��  � �� 5r      �� 5s      8  6*   5u      5v      P� 5w       �@��
IsReadOnly$
 True��6,   . �5x      ��  � �� 5y      �� 5z      8 	 6*   5|      5}      5�      �� 5�      " ��6%   .�5�   
   ��  � �� 5�      �� 5�      8   6"   5�      �� 5�   
    � �� 5�      �� 5�      8  6"   5�      �� 5�   
    � ��$	 8 4q�6$   �� 5�   
    � ��$ Stretch=�60   �� 5�   
    � ��$
 Left=�67   �� 5�   
    � ��$ Stretch=�6.   �� 5�   
    � ��$ Center=�65   �� 5�   
    � ��$ 1q�6,   �� 5�   
    �@=�ErrorTemplate8  65   �� 5�   
    � �� 5�      �� 5�      8  6"   5�      �� 5�   
    � ��8  6%   5�      �� 5�      " ��6%   8   6C   5�      