import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'theme.dart';

class ThemeProvider extends ChangeNotifier {
  ThemeData _currentTheme = AppThemes.lightTheme;
  ThemeData _themeA = AppThemes.lightTheme; // Default Theme A
  ThemeData _themeB = AppThemes.darkTheme;  // Default Theme B
  int _primaryThemeIndex = 0; // Index for Theme A
  int _secondaryThemeIndex = 1; // Index for Theme B

  ThemeData get currentTheme => _currentTheme;
  ThemeData get themeA => _themeA;
  ThemeData get themeB => _themeB;
  int get primaryThemeIndex => _primaryThemeIndex;
  int get secondaryThemeIndex => _secondaryThemeIndex;

  void setPrimaryThemeIndex(int index) {
    _primaryThemeIndex = index;
    _themeA = _getThemeByIndex(index);
    notifyListeners();
  }

  void setSecondaryThemeIndex(int index) {
    _secondaryThemeIndex = index;
    _themeB = _getThemeByIndex(index);
    notifyListeners();
  }

  void applyTheme(ThemeData theme) {
    // Directly set the current theme and notify listeners
    _currentTheme = theme;
    _updateSystemUI();
    notifyListeners();
  }

  void toggleBetweenThemes() {
    _currentTheme = _currentTheme == _themeA ? _themeB : _themeA;
    _updateSystemUI();
    notifyListeners();
  }

  void switchThemes() {
    // Swap Primary and Secondary themes
    final tempTheme = _themeA;
    final tempIndex = _primaryThemeIndex;

    _themeA = _themeB;
    _themeB = tempTheme;
    _primaryThemeIndex = _secondaryThemeIndex;
    _secondaryThemeIndex = tempIndex;

    toggleBetweenThemes();
  }

  void _updateSystemUI() {
    SystemChrome.setSystemUIOverlayStyle(
      SystemUiOverlayStyle(
        systemNavigationBarColor: _currentTheme.brightness == Brightness.dark
            ? Colors.black
            : Colors.white,
        systemNavigationBarIconBrightness: _currentTheme.brightness == Brightness.dark
            ? Brightness.light
            : Brightness.dark,
        statusBarColor: _currentTheme.brightness == Brightness.dark
            ? Colors.black
            : Colors.white,
        statusBarIconBrightness: _currentTheme.brightness == Brightness.dark
            ? Brightness.light
            : Brightness.dark,
      ),
    );
  }

  ThemeData _getThemeByIndex(int index) {
    switch (index) {
      case 0:
        return AppThemes.lightTheme;
      case 1:
        return AppThemes.darkTheme;
      case 2:
        return AppThemes.customTheme;
      default:
        return AppThemes.lightTheme;
    }
  }
}
