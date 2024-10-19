import {initializeApp} from 'firebase/app'
// import {getAuth} from 'firebase/auth'
import {getDatabase} from 'firebase/database'

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
const app = initializeApp(firebaseConfig)
export const db = getDatabase(app)
