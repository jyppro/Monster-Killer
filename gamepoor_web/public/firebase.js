// Firebase configuration
const firebaseConfig = {
  apiKey: 'AIzaSyCQHYOxAJ6LIM17ylSnHi2FrRrvUa-n_14',
  authDomain: 'database-2fda2.firebaseapp.com',
  databaseURL: 'https://database-2fda2-default-rtdb.firebaseio.com',
  projectId: 'database-2fda2',
  storageBucket: 'database-2fda2.appspot.com',
  messagingSenderId: '531669849508',
  appId: '1:531669849508:web:3ade010326c93f16fd9d31',
  measurementId: 'G-JKT7RWKTTD'
}

// Initialize Firebase
//경고보내서 임시적으로 경고문 제외 실행상 문제는 없음.
/* eslint-disable */
firebase.initializeApp(firebaseConfig)
/* eslint-disable */

// Firebase Authentication
// const auth = firebase.auth()

// Firebase Database
// const database = firebase.database()

// Export to global window object if needed in other parts of the app
// window.firebaseApp = {auth, database}

/*
한 줄의 경고 무시: // eslint-disable-next-line
특정 규칙만 무시: // eslint-disable-next-line 규칙명
// 코드 블록 전체 무시: /* eslint-disable */ //및 /* eslint-enable */
//파일 전체 무시: /* eslint-disable */ (파일의 맨 위에 위치)
//*/
