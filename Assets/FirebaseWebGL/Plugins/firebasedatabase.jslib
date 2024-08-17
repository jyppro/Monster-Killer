mergeInto(LibraryManager.library, {
    LoadGameData: function (playerID, objectName, callback, fallback){
        var parsedPath = "players" + '/' + playerID;
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

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
    },
    SaveGameData: function(playerID, 
    rank, 
    power, 
    maxHP, 
    currentHP, 
    gold, 
    sumScore, 
    time, 
    objectName, callback, fallback){
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        var data = {
            playerID: playerID,
            rank: rank,
            power: power,
            maxHP: maxHP,
            currentHP: currentHP,
            gold: gold,
            sumScore: sumScore,
            time: time
        };

        var parsedPath = 'players/' + playerID;
        var parsedValue = JSON.stringify(data);

        try {
            firebase.database()
            .ref(parsedPath)
            .set(data).then(function() {
                window.unityInstance.SendMessage(parsedObjectName,
                 parsedCallback,
                  "Success: " + parsedValue + " was saveed to " + parsedPath);
            }).catch(function(error){
                window.unityInstance.SendMessage(parsedObjectName,
                 parsedFallback,
                 JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });

        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName,
             parsedFallback,
              JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    }
});