mergeInto(LibraryManager.library, {
    LoadGameData: function (playerID, objectName, callback, fallback){
        var parsedPath = "players" + '/' + playerID;
        var parsedObjectName = Pointer_stringify(objectName);
        var parsedCallback = Pointer_stringify(callback);
        var parsedFallback = Pointer_stringify(fallback);

        try {
        firebase.database().ref(parsedPath)
        .once('value')
        .then(function(snapshot) {
            var playerID = snapshot.child("playerID").val();
            var rank = snapshot.child("rank").val();
            var power = snapshot.child("power").val();
            var gold = snapshot.child("gold").val();
            var time = snapshot.child("time").val();
            var maxHP = snapshot.child("maxHP").val();
            var data = {
                playerID: playerID,
                rank: rank,
                power: power,
                gold: gold,
                time: time,
                maxHP: maxHP
            };
            window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(data));
        }).catch(function(error){
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName,parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    }
});