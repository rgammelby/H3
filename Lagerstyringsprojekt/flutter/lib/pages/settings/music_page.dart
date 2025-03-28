import 'package:flutter/material.dart';
import '../../native_audio.dart';
import '../../services/preferences/preferences_service.dart';
import '../../services/global/toast_service.dart';
import '../../services/global/localization.dart';

class MusicPage extends StatefulWidget {
  final PreferencesService preferencesService;

  const MusicPage({super.key, required this.preferencesService});

  @override
  _MusicPageState createState() => _MusicPageState();
}

class _MusicPageState extends State<MusicPage> {
  late bool _playBGM;
  bool _preferencesChanged = false; // Track if preferences have been modified
  late bool _originalPlayBGM; // Backup for original preference

  @override
  void initState() {
    super.initState();
    _playBGM = widget.preferencesService.playBGM; // Load initial BGM preference
    _originalPlayBGM = _playBGM; // Backup the original state
  }

  void _revertPreferences() {
    // Revert to original values
    setState(() {
      _playBGM = _originalPlayBGM;
    });
    ToastService.showCustomToast(
      context,
      AppLocalizations.of(context).translate('changes_discarded'),
      backgroundColor: Colors.orange,
    );
  }

  Future<void> _toggleBGM(bool enable) async {
    setState(() {
      _playBGM = enable; // Update state
      _preferencesChanged = true; // Mark preferences as changed
    });
  }

  Future<void> _savePreferences() async {
    try {
      widget.preferencesService.playBGM = _playBGM; // Save the updated preference
      await widget.preferencesService.savePreferences();

      // Show success toast
      ToastService.showSuccessToast(
        context,
        AppLocalizations.of(context).translate('preferences_saved'),
      );
      setState(() {
        _preferencesChanged = false; // Reset change tracker
        _originalPlayBGM = _playBGM; // Update original state
      });
    } catch (e) {
      // Show error toast in case of failure
      ToastService.showErrorToast(
        context,
        AppLocalizations.of(context).translate('save_preferences_failed'),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    final localizations = AppLocalizations.of(context);

    return WillPopScope(
      onWillPop: () async {
        if (_preferencesChanged) {
          _revertPreferences(); // Revert unsaved changes
        }
        return true; // Allow navigation to proceed
      },
      child: Scaffold(
        appBar: AppBar(
          title: Text(localizations.translate('music_page_title')),
        ),
        body: Padding(
          padding: const EdgeInsets.all(16.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.start,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              // Toggle BGM Switch
              SwitchListTile(
                title: Text(localizations.translate('enable_bgm')),
                value: _playBGM,
                onChanged: _toggleBGM,
              ),
              const SizedBox(height: 16.0),

              // Play BGM Button (only visible if enabled)
              if (_playBGM)
                ElevatedButton.icon(
                  onPressed: () async {
                    try {
                      await NativeAudio.playBGM();
                      ToastService.showSuccessToast(
                        context,
                        localizations.translate('bgm_started'),
                      );
                    } catch (e) {
                      ToastService.showErrorToast(
                        context,
                        localizations.translate('bgm_start_failed'),
                      );
                    }
                  },
                  icon: const Icon(Icons.play_arrow),
                  label: Text(localizations.translate('play_bgm')), // Localized button text
                  style: ElevatedButton.styleFrom(backgroundColor: Colors.green),
                ),

              if (_playBGM)
                const SizedBox(height: 16.0),
              if (_playBGM)
                ElevatedButton.icon(
                  onPressed: () async {
                    try {
                      await NativeAudio.stopBGM();
                      ToastService.showSuccessToast(
                        context,
                        localizations.translate('bgm_stopped'),
                      );
                    } catch (e) {
                      ToastService.showErrorToast(
                        context,
                        localizations.translate('bgm_stop_failed'),
                      );
                    }
                  },
                  icon: const Icon(Icons.stop),
                  label: Text(localizations.translate('stop_bgm')), // Localized button text
                  style: ElevatedButton.styleFrom(backgroundColor: Colors.red),
                ),

              const Divider(), // Separate existing functionality from future additions

              // Future Feature: Add Music
              TextButton(
                onPressed: () {
                  ToastService.showCustomToast(
                    context,
                    localizations.translate('add_music_coming_soon'),
                    backgroundColor: Colors.blueGrey,
                  );
                },
                child: Text(localizations.translate('add_music')), // Localized button text
              ),
              const SizedBox(height: 16.0),

              // Future Feature: Create Playlist
              TextButton(
                onPressed: () {
                  ToastService.showCustomToast(
                    context,
                    localizations.translate('create_playlist_coming_soon'),
                    backgroundColor: Colors.blueGrey,
                  );
                },
                child: Text(localizations.translate('create_playlist')), // Localized button text
              ),
              const SizedBox(height: 16.0),

              // Future Feature: View Playlists
              TextButton(
                onPressed: () {
                  ToastService.showCustomToast(
                    context,
                    localizations.translate('view_playlists_coming_soon'),
                    backgroundColor: Colors.blueGrey,
                  );
                },
                child: Text(localizations.translate('view_playlists')), // Localized button text
              ),
              const SizedBox(height: 16.0),

              // Future Feature: Pick a Playlist
              TextButton(
                onPressed: () {
                  ToastService.showCustomToast(
                    context,
                    localizations.translate('pick_playlist_coming_soon'),
                    backgroundColor: Colors.blueGrey,
                  );
                },
                child: Text(localizations.translate('pick_playlist')), // Localized button text
              ),

              const Spacer(), // Push "Save Preferences" button to the bottom

              // Save Preferences Button
              ElevatedButton.icon(
                onPressed: _preferencesChanged ? _savePreferences : null,
                icon: const Icon(Icons.save),
                label: Text(localizations.translate('save_preferences')), // Localized button text
                style: ElevatedButton.styleFrom(
                  minimumSize: const Size(double.infinity, 50), // Wide button style
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
