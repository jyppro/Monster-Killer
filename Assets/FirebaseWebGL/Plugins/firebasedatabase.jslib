mergeInto(LibraryManager.library, {
    LoadGameData: function (playerID, objectName, callback, fallback){
        var parsedPath = 'players/' + playerID;
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        console.log('데이터경로 보기'+ parsedPath);
        console.log('플레이어 아이디 보기'+ playerID);

        try {
        firebase.database().ref(parsedPath)
        .once('value')
        .then(function(snapshot) {
            var rank = snapshot.child("rank").val();
            var power = snapshot.child("power").val();
            var gold = snapshot.child("gold").val();
            var time = snapshot.child("time").val();
            var maxHP = snapshot.child("maxHP").val();
            var currentHP = snapshot.child("currentHP").val();
            var sumScore = snapshot.child("sumScore").val();
            var data = {
                playerID: playerID,
                rank: rank,
                power: power,
                gold: gold,
                time: time,
                maxHP: maxHP,
                currentHP: currentHP,
                sumScore: sumScore
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
            .update(data).then(function() {
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