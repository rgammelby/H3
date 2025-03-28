import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:xml/xml.dart';

class AppLocalizations {
  final Locale locale;
  late Map<String, String> _localizedStrings;

  AppLocalizations(this.locale);

  // Loads the XML file for the selected locale
  Future<void> load() async {
    try {
      debugPrint("Attempting to load XML file for locale: ${locale.languageCode}");

      // Load the XML file
      final String xmlContent = await rootBundle.loadString('assets/lang/${locale.languageCode}.xml');
      debugPrint("File content loaded for ${locale.languageCode}");

      // Parse the XML
      final document = XmlDocument.parse(xmlContent);
      debugPrint("Parsing XML content for locale: ${locale.languageCode}");

      // Extract string name-value pairs
      _localizedStrings = {
        for (var element in document.findAllElements('string'))
          element.getAttribute('name')?.trim() ?? '': element.text.trim(),
      };

      debugPrint("Parsed keys for locale ${locale.languageCode}: ${_localizedStrings.keys}");
    } catch (e) {
      debugPrint("Error loading XML for ${locale.languageCode}: $e");
      await _loadFallbackTranslations();
    }
  }

  // Loads English translations as a fallback
  Future<void> _loadFallbackTranslations() async {
    try {
      debugPrint("Loading fallback translations (English)");

      final fallbackXmlContent = await rootBundle.loadString('assets/lang/en.xml');
      final fallbackDocument = XmlDocument.parse(fallbackXmlContent);

      _localizedStrings = {
        for (var element in fallbackDocument.findAllElements('string'))
          element.getAttribute('name')?.trim() ?? '': element.text.trim(),
      };

      debugPrint("Fallback translations loaded successfully");
    } catch (e) {
      debugPrint("Error loading fallback XML: $e");
      _localizedStrings = {};
    }
  }

  // Retrieves the translation for a given key
  String translate(String key) {
    if (_localizedStrings.containsKey(key)) {
      debugPrint("Translation found for key: $key -> ${_localizedStrings[key]}");
      return _localizedStrings[key]!;
    } else {
      debugPrint("Missing translation for key: $key in locale: ${locale.languageCode}");
      return '[[$key]]'; // Return the key in brackets for debugging
    }
  }

  // Access localization strings via BuildContext
  static AppLocalizations of(BuildContext context) {
    final localizations = Localizations.of<AppLocalizations>(context, AppLocalizations);
    assert(localizations != null, 'AppLocalizations is null');
    debugPrint("AppLocalizations instance retrieved");
    return localizations!;
  }

  // Delegate for AppLocalizations
  static const LocalizationsDelegate<AppLocalizations> delegate = _AppLocalizationsDelegate();
}

// Localization delegate for AppLocalizations
class _AppLocalizationsDelegate extends LocalizationsDelegate<AppLocalizations> {
  const _AppLocalizationsDelegate();

  @override
  bool isSupported(Locale locale) {
    debugPrint("Checking if locale is supported: ${locale.languageCode}");
    return ['en', 'ja', 'da'].contains(locale.languageCode);
  }

  @override
  Future<AppLocalizations> load(Locale locale) async {
    debugPrint("Loading localization delegate for: ${locale.languageCode}");

    final localizations = AppLocalizations(locale);
    await localizations.load(); // Load translations for the given locale
    return localizations;
  }

  @override
  bool shouldReload(covariant LocalizationsDelegate<AppLocalizations> old) {
    debugPrint("Checking if delegate reload is needed");
    return false; // No need to reload for static localization files
  }
}
