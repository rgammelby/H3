import 'dart:io';
import 'package:xml/xml.dart' as xml;

class LanguagePreferences {
  String selectedLanguage = 'en'; // Default language
  final String filePath = '/data/user/0/com.example.bestworstapp/files/language_preferences.xml';

  Future<void> load() async {
    final file = File(filePath);

    if (await file.exists()) {
      try {
        final xmlContent = await file.readAsString();
        final document = xml.XmlDocument.parse(xmlContent);
        selectedLanguage = document.getElement('preferences')
            ?.getElement('selectedLanguage')
            ?.text ??
            'en'; // Default to 'en' if not found
      } catch (e) {
        print('Error loading language preferences: $e');
      }
    }
  }

  Future<void> save() async {
    final builder = xml.XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('preferences', nest: () {
      builder.element('selectedLanguage', nest: selectedLanguage);
    });

    final document = builder.buildDocument();
    final file = File(filePath);

    try {
      await file.writeAsString(document.toXmlString(pretty: true));
      print('Language preferences saved to: $filePath');
    } catch (e) {
      print('Error saving language preferences: $e');
    }
  }
}
