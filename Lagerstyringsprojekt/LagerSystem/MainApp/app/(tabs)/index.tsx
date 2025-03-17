import ImageViewer from "@/components/ImageViewer";
import Button from "@/components/Button";
import { View, StyleSheet } from "react-native";
import * as ImagePicker from "expo-image-picker";
import { type ImageSource } from 'expo-image';
import { useState } from "react";
import IconButton from "@/components/IconButton";
import CircleButton from "@/components/CircleButton";
import EmojiPicker from "@/components/EmojiPicker";
import EmojiList from '@/components/EmojiList';
import EmojiSticker from '@/components/EmojiSticker';

const placeHolderImage = require('@/assets/images/welcome.png');

export default function Index() {
  const [selectedImage, setSelectedImage] = useState<string | undefined>(undefined);
  const [showAppOptions, setShowAppOptions] = useState<boolean>(false);
  const [isModalSiviable, setIsModalVisiable] = useState<boolean>(false);
  const [pickedEmoji, setPickedEmoji] = useState<ImageSource | undefined>(undefined);

  const pickImageAsync = async () => {
    let result = await ImagePicker.launchImageLibraryAsync({
      mediaTypes: ['images'],
      allowsEditing: true,
      quality: 1,
    });

    if (!result.canceled) {
      setSelectedImage(result.assets[0].uri);
      setShowAppOptions(true);
    } else {
      alert('You did not select any image');
    }
  }

  const onReset = () => {
    setShowAppOptions(false);
  };

  const onAddSticker = () => {
    setIsModalVisiable(true);
  };
  const onModalClose = () => {
    setIsModalVisiable(false);
  }

  const onSaveImageAsync = () => { 
    alert('Save image');
  };

  return (
    <View style={Styles.container} >
      
      <View style={Styles.imageContainer}>
        {/* It passes the placeholder (PlaceholderImage).
        It also passes the user-selected image (selectedImage).
        ImageViewer decides which one to show based on: 
        onst imageSource = selectedImage ? { uri: selectedImage } : imgSource; */}

        <ImageViewer imgSource={placeHolderImage} selectedImage={selectedImage} />
        {pickedEmoji && <EmojiSticker imageSize={40} stickerSource={pickedEmoji} />}
      </View>
      
      {showAppOptions ? (
        <View style={Styles.optionsContainer} >
          <View style={Styles.optionsRow}>
            <IconButton icon="refresh" label="Reset" onPress={onReset} />
            <CircleButton onPress={onAddSticker} />
            <IconButton icon="save-alt" label="Save" onPress={onSaveImageAsync} />
          </View>
        </View>
      ) : (
        <View style={Styles.footerContainer}>
          {/* <Button theme="primary" label="Choose a photo" onPress={pickImageAsync} /> */}
          <Button theme="primary" label="Welcome!" onPress={pickImageAsync} />
          <Button label="Go to Devices to find the device!" onPress={()=>setShowAppOptions(true)} />
      </View>
      )}
      
      <EmojiPicker isVisible={isModalSiviable} onClose={onModalClose}>
        {/* A list of emoji component will go here */}
        <EmojiList onSelect={setPickedEmoji} onCloseModal={onModalClose} />
      </EmojiPicker>
    </View>
  );
}

const Styles = StyleSheet.create({
  container: {
    flex: 1, 
    justifyContent: "center",
    alignItems: "center",
    backgroundColor: "#25292e",
  },
  imageContainer: {
    flex: 1, 
  },
  footerContainer: {
    flex: 1 / 3, 
    alignItems: 'center',
  },
  optionsContainer: {
    position: 'absolute',
    bottom: 80,
  },
  optionsRow: {
    alignItems: 'center',
    flexDirection: 'row',
  },
});

