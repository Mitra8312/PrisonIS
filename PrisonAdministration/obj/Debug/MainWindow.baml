   M S B A M L   `   `   `  ����O  KPrisonAdministration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PrisonAdministrationPrisonAdministration  5   	   %    PrisonAdministration.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:PrisonAdministration     5   	   -   5          � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5   
   �� $��2��6   $��1��6    $��2��6-   $2��,Логин администратора БД��6A   $��27(�6c   $��Center=�6q   �� 5   
   -   5        $��tbLoginUser��5   C   $��2��6   $��2��6   $��2��6/   $��23(�6V    �� TextAlignment$ Center=�6d   �� 5   
   $��4��6   $��2��6   $��2��6-   $��Center=�6A   $4��.Пароль администратора БД��6^   $��27(�6�   G� 5   
   -   5        $��tbPasswordUser��5   G   $��5��6   $��2��6#   $��2��63   $��16(�6]    G� PasswordChar$	 ❤��6k   �� 5   
   -   5        $��
AuthButton��5   f   $��2��6   $��7��6!   $��2��6.   $��Авторизация��6B   $��27(�6X   6x    � RowDefinitions 5   
   �� 5      $	��24* �6   �� 5      $	��59* �6   �� 5      $	��45* �6   �� 5      $	��45* �6   �� 5      $	��59* �6   �� 5      $	��45* �6   �� 5      $	��45* �6   �� 5      $	��53* �6   �� 5      $	��51* �6   �� 5      $	��24* �6    � ColumnDefinitions 5!   
   �� 5"      $	��26* �6   �� 5#      $
��185* �6   �� 5$      $
��192* �6   �� 5%      $
��192* �6   �� 5&      $
��185* �6   �� 5'      $	��26* �6   5*      5+      