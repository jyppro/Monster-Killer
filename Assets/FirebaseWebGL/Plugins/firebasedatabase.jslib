mergeInto(LibraryManager.library, {
  LoadGameData: function (playerID, objectName, callback, fallback) {
    var playerIDStr = UTF8ToString(playerID);
    var parsedPath = "players/" + playerIDStr;
    var parsedObjectName = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);
    console.log("데이터경로 보기" + parsedPath);
    console.log("플레이어 아이디 보기" + playerIDStr);

    try {
      firebase
        .database()
        .ref(parsedPath)
        .once("value")
        .then(function (snapshot) {
          var rank = snapshot.child("rank").val();
          var power = snapshot.child("power").val();
          var gold = snapshot.child("gold").val();
          var time = snapshot.child("time").val();
          var maxHP = snapshot.child("maxHP").val();
          var currentHP = snapshot.child("currentHP").val();
          var sumScore = snapshot.child("sumScore").val();
          var data = {
            playerID: playerIDStr,
            rank: rank,
            power: power,
            gold: gold,
            time: time,
            maxHP: maxHP,
            currentHP: currentHP,
            sumScore: sumScore,
          };
          window.unityInstance.SendMessage(
            parsedObjectName,
            parsedCallback,
            JSON.stringify(data)
          );
        })
        .catch(function (error) {
          window.unityInstance.SendMessage(
            parsedObjectName,
            parsedFallback,
            JSON.stringify(error, Object.getOwnPropertyNames(error))
          );
        });
    } catch (error) {
      window.unityInstance.SendMessage(
        parsedObjectName,
        parsedFallback,
        JSON.stringify(error, Object.getOwnPropertyNames(error))
      );
    }
  },
  SaveGameData: function (
    playerID,
    rank,
    power,
    maxHP,
    currentHP,
    gold,
    sumScore,
    time,
    objectName,
    callback,
    fallback
  ) {
    var playerIDStr = UTF8ToString(playerID);
    var parsedObjectName = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);

    var data = {
      playerID: playerIDStr,
      rank: rank,
      power: power,
      maxHP: maxHP,
      currentHP: currentHP,
      gold: gold,
      sumScore: sumScore,
      time: time,
    };

    var parsedPath = "players/" + playerIDStr;
    var parsedValue = JSON.stringify(data);

    try {
      firebase
        .database()
        .ref(parsedPath)
        .update(data)
        .then(function () {
          window.unityInstance.SendMessage(
            parsedObjectName,
            parsedCallback,
            "Success: " + parsedValue + " was saveed to " + parsedPath
          );
        })
        .catch(function (error) {
          window.unityInstance.SendMessage(
            parsedObjectName,
            parsedFallback,
            JSON.stringify(error, Object.getOwnPropertyNames(error))
          );
        });
    } catch (error) {
      window.unityInstance.SendMessage(
        parsedObjectName,
        parsedFallback,
        JSON.stringify(error, Object.getOwnPropertyNames(error))
      );
    }
  },

  LoadRankingsData: function (objectName, callback, fallback) {
    // 문자열 변환
    const parsedObjectName = UTF8ToString(objectName);
    const parsedCallback = UTF8ToString(callback);
    const parsedFallback = UTF8ToString(fallback);
    const parsedPath = "sorted_rankings"; // Firebase 경로

    console.log("데이터 경로:", parsedPath);

    try {
      // Firebase에서 sorted_rankings 데이터 가져오기
      firebase
        .database()
        .ref(parsedPath)
        .once("value")
        .then(function (snapshot) {
          if (snapshot.exists()) {
            const rankingsData = snapshot.val();

            // 데이터를 배열 형태로 변환
            const rankingsArray = Object.keys(rankingsData).map((key) => ({
              rank_PlayerID: rankingsData[key].playerID,
              rank_Name: rankingsData[key].name,
              rank_Score: rankingsData[key].score,
              rank_Rank: rankingsData[key].rank,
            }));

            // Unity로 데이터 전송
            window.unityInstance.SendMessage(
              parsedObjectName,
              parsedCallback,
              JSON.stringify(rankingsArray)
            );
          } else {
            // 데이터가 없을 경우 처리
            window.unityInstance.SendMessage(
              parsedObjectName,
              parsedFallback,
              JSON.stringify({ error: "No data found" })
            );
          }
        })
        .catch(function (error) {
          // Firebase 에러 처리
          window.unityInstance.SendMessage(
            parsedObjectName,
            parsedFallback,
            JSON.stringify({ message: error.message, code: error.code || "Unknown" })
          );
        });
    } catch (error) {
      // 일반적인 오류 처리
      window.unityInstance.SendMessage(
        parsedObjectName,
        parsedFallback,
        JSON.stringify({ message: error.message, code: "Unknown" })
      );
    }
  },
});
