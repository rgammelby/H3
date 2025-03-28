import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'shake_toggle_theme.dart';
import '../../theme/theme_provider.dart';
import '../preferences/preferences_service.dart';

class ThemeManager {
  static void linkShakeToTheme(BuildContext context, ShakeToggleTheme shakeService) {
    shakeService.onShakeDetected = () async {
      final themeProvider = Provider.of<ThemeProvider>(context, listen: false);

      // Switch themes when shake is detected
      themeProvider.switchThemes();
      print('Shake detected: Themes switched!');

      // Get the current language from PreferencesService
      final preferencesService = PreferencesService();
      await preferencesService.loadPreferences(); // Ensure preferences are up-to-date
      final currentLanguage = preferencesService.selectedLanguage;

      // Save updated preferences with the current language
      preferencesService.isShakeEnabled = shakeService.isShakeEnabled;
      preferencesService.themeA = themeProvider.primaryThemeIndex;
      preferencesService.themeB = themeProvider.secondaryThemeIndex;
      preferencesService.selectedLanguage = currentLanguage;

      await preferencesService.savePreferences();
    };

    shakeService.startListening();
  }
}
