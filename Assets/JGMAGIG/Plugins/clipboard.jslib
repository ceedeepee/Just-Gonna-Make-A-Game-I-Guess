mergeInto(LibraryManager.library, {
  CopyToClipboard: function (textPtr) {
    var text = Pointer_stringify(textPtr);
    navigator.clipboard.writeText(text);
  },

  PasteFromClipboard: function () {
    return navigator.clipboard.readText().then(function (text) {
      var buffer = _malloc(lengthBytesUTF8(text) + 1);
      stringToUTF8(text, buffer, lengthBytesUTF8(text) + 1);
      return buffer;
    });
  }
});
