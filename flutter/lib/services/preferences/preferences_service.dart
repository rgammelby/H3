import 'dart:io';
import 'package:xml/xml.dart' as xml;

class PreferencesService {
  bool isShakeEnabled = true;
  int themeA = 0;
  int themeB = 1;
  String selectedLanguage = 'en';
  bool playBGM = true;

  final String filePath = '/data/user/0/com.example.bestworstapp/files/preferences.xml';

  /// Loads preferences from the XML file, or sets defaults if the file doesn't exist
  Future<void> loadPreferences() async {
    final file = File(filePath);

    if (await file.exists()) {
      try {
        final xmlContent = await file.readAsString();
        final document = xml.XmlDocument.parse(xmlContent);

        isShakeEnabled = document.getElement('preferences')
            ?.getElement('isShakeEnabled')
            ?.text
            .toLowerCase() ==
            'true';
        themeA = int.tryParse(
            document.getElement('preferences')?.getElement('themeA')?.text ??
                '0') ??
            0;
        themeB = int.tryParse(
            document.getElement('preferences')?.getElement('themeB')?.text ??
                '1') ??
            1;
        selectedLanguage = document
            .getElement('preferences')
            ?.getElement('selectedLanguage')
            ?.text ??
            'en';
        playBGM = document.getElement('preferences')
            ?.getElement('playBGM')
            ?.text
            .toLowerCase() ==
            'true';
      } catch (e) {
        print('Error loading preferences: $e');
      }
    } else {
      print('Preferences file does not exist. Creating new file with default values.');
      await savePreferences();
    }
  }

  /// Saves the current preferences to the XML file
  Future<void> savePreferences() async {
    final builder = xml.XmlBuilder();
    builder.processing('xml', 'version="1.0" encoding="UTF-8"');
    builder.element('preferences', nest: () {
      builder.element('isShakeEnabled', nest: isShakeEnabled.toString());
      builder.element('themeA', nest: themeA.toString());
      builder.element('themeB', nest: themeB.toString());
      builder.element('selectedLanguage', nest: selectedLanguage);
      builder.element('playBGM', nest: playBGM.toString());
    });

    final document = builder.buildDocument();
    final file = File(filePath);

    try {
      await file.writeAsString(document.toXmlString(pretty: true));
      print('Preferences saved to: $filePath');
    } catch (e) {
      print('Error saving preferences: $e');
    }
  }
}
