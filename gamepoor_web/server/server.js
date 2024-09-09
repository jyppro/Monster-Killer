const express = require('express');
const mongoose = require('mongoose');
const session = require('express-session');
const path = require('path');
const cors = require('cors');
const userRoutes = require('./routes/user');
const app = express();
const port = 5000;

// MongoDB 연결 설정
mongoose.connect('mongodb+srv://admin:1234@cluster0.lybmjvg.mongodb.net/gamedata', {
  useNewUrlParser: true,
  useUnifiedTopology: true,
}).then(() => {
  console.log('MongoDB에 연결 성공');
}).catch((error) => {
  console.error('MongoDB 연결 오류:', error);
});

//cors 설정
app.use(cors({
  origin: 'http://localhost:3000',  
  credentials: true                
}));

//세션관련 설정 중 401에러가 고쳐지지 않고있음.
app.use(session({
  secret: 'your_secret_key',
  resave: false,
  saveUninitialized: true,
  cookie: {
    secure: false,  
    httpOnly: true, 
    maxAge: 86400000 
  }
}));


// 미들웨어 설정
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// 라우트 설정
app.use('/api/users', userRoutes);

// 정적 파일 서빙 설정
app.use('/build', express.static(path.join(__dirname, 'build')));
app.use('/TemplateData', express.static(path.join(__dirname, 'TemplateData')));
app.use(express.static(path.join(__dirname, '../client/build'))); // React 빌드 폴더 서빙

// 모든 경로에 대해 동일한 HTML 파일 서빙
app.get('*', (req, res) => {
  res.sendFile(path.join(__dirname, '../client/build', 'index.html'));
});

// 서버 시작
app.listen(port, () => {
  console.log(`서버가 http://localhost:${port} 에서 실행 중입니다.`);
});
