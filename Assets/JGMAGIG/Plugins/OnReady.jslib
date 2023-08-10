mergeInto(LibraryManager.library, {
  OnReady: function (text) {
    try {
      window.dispatchReactUnityEvent("OnReady", UTF8ToString(text));
    } catch (e) {
      console.warn("Failed to dispatch OnReady event");
    }
  },
  MintBonkDog: function (text) {
    try {
      window.dispatchReactUnityEvent("MintBonkDog", UTF8ToString(text));
    } catch (e) {
      console.warn("Failed to dispatch MintBonkDog event");
    }
  },      
  SendReactMessage: function (textFunction, textMessage) {
    try {
      window.dispatchReactUnityEvent(UTF8ToString(textFunction), UTF8ToString(textMessage));
    } catch (e) {
      console.warn("Failed to dispatch SendMessage event", textFunction, textMessage);
    }
  }, 
  SendFinishMessage: function (text) {
    try {
      window.dispatchReactUnityEvent("finish", UTF8ToString(text));
    } catch (e) {
      console.warn("Failed to dispatch SendMessage event", text);
    }
  },    
  UnityPinReceived: function (text) {
    try {
      window.dispatchReactUnityEvent("UnityPinReceived", UTF8ToString(text));
    } catch (e) {
      console.warn("Failed to dispatch UnityPinReceived event");
    }
  },
  OpenNewTab : function(url)
  {
      url = Pointer_stringify(url);
      window.open(url,'_blank');
  },
});