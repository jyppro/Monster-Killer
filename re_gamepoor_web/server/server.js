// 라이브러리 불러오기
const express = require('express');
const session = require('express-session');
const path = require('path');
const app = express();
const http = require('http').createServer(app);

const authRoutes = require('./routes/authRoutes');
// env file read
const mongoose = require('mongoose');
// index.js 자동참조
const { PORT, MONGO_URI, SESSION_SECRET } = require('./config');
// const connectDB = require('./config/db');

// 미들웨어 설정
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
// 라우트 설정
app.use('/api/auth', authRoutes);

app.use(session({
  secret: SESSION_SECRET,
  resave: false,
  saveUninitialized: true,
}));

// MongoDB 연결 설정
try {
      mongoose.connect(MONGO_URI, {
          // useNewUrlParser: true,
          // useUnifiedTopology: true,
      });
      console.log('MongoDB에 연결 성공');
} catch (err) {
      console.error('MongoDB 연결 오류:', err);
      //process.exit(1);
}

// 8080번 포트에서 서버를 실행할거야
http.listen(PORT, () => {
  // 서버가 정상적으로 실행되면 콘솔창에 이 메시지를 띄워줘
  console.log(`server 파일 ${PORT}포트에서 정상 실행중`);
});

//build 파일이 생성되면, npm run build, 정적 파일로 실행
app.use(express.static(path.join(__dirname, '../client/build')));

// 메인페이지 접속 시 build 폴더의 index.html 보내줘
app.get('/', (res, req) => {
  req.sendFile(path.join(__dirname, '../client/build/index.html'));
})

//라우팅은 리액트가 담당, react-router-dom, 항상 하단에 위치
app.get('*', (res, req) => {
    req.sendFile(path.join(__dirname, '../client/build/index.html'));
});