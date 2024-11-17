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

  SaveStagesClearedData: function (
    playerID,
    stagesClearedJSON,
    objectName,
    callback,
    fallback
  ) {
    var playerIDStr = UTF8ToString(playerID);
    var parsedObjectName = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);
    var stagesCleared;

    try{
      stagesCleared = JSON.parse(UTF8ToString(stagesClearedJSON)); // 배열 파싱
      console.log("[Save]StagesCleared: " + stagesCleared);
      console.log("[Save]StagesClearedJSON: " + UTF8ToString(stagesClearedJSON));
    } catch (error){
      window.unityInstance.SendMessage(
        parsedObjectName,
        parsedFallback,
        "Failed to parse stagesCleared: " + error
      );
      return;
    }

    // 배열을 객체 형식으로 변환
    var stagesClearedObject = stagesCleared.reduce(function (obj, value, index) {
      obj[index] = value;
      return obj;
    }, {});

    // 데이터 구조 생성
    var data = {
      stagesClearedList: stagesClearedObject, // 객체 형태로 저장
    };

    var parsedPath = "players/" + playerIDStr;
    // var parsedValue = JSON.stringify(data);

    try {
      firebase
        .database()
        .ref(parsedPath)
        .update(data)
        .then(function () {
          window.unityInstance.SendMessage(
            parsedObjectName,
            parsedCallback,
            "Success: " + JSON.stringify(data) + "Data saved to stagesClearList : " + parsedPath
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

  LoadStagesClearedData: function (
    playerID,
    objectName,
    callback,
    fallback
  ) {
    var playerIDStr = UTF8ToString(playerID);
    var parsedObjectName = UTF8ToString(objectName);
    var parsedCallback = UTF8ToString(callback);
    var parsedFallback = UTF8ToString(fallback);

    var parsedPath = "players/" + playerIDStr + "/stagesClearedList";

    try {
      firebase
        .database()
        .ref(parsedPath)
        .once("value")
        .then(function (snapshot) {
          // 배열 데이터 가져오기
          var stagesClearedList = snapshot.val();
          console.log("stagesClearedList : ", stagesClearedList);

          if (stagesClearedList === null) {
            console.log("경로에 데이터가 없습니다." + parsedPath);
            stagesClearedList = []; // 데이터가 없으면 빈 배열
          } else {
            console.log("데이터가 있습니다. stagesClearedList:", stagesClearedList);
          }

          // 데이터가 객체 형식으로 되어 있을 경우 배열로 변환
          if (stagesClearedList && typeof stagesClearedList === 'object') {
            // 객체를 배열로 변환
            stagesClearedList = Object.values(stagesClearedList);
            console.log("Converted to array:", stagesClearedList);
          }

          var data = {
            stagesCleared: stagesClearedList
          };

          // JSON 데이터를 Unity로 보내기 전에 문자열로 변환
          var jsonString = JSON.stringify(data);
          console.log("스테이지 데이터 변환 확인용:: ", jsonString);

          window.unityInstance.SendMessage(
            parsedObjectName,
            parsedCallback,
            jsonString
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
});
