// server.js
const express = require('express');
const path = require('path');

const app = express();
const port = 8080; // 원하는 포트 번호 설정

// 유니티 WebGL 빌드 파일이 위치한 디렉토리를 지정
//const buildPath = path.join(__dirname, 'Build_WebGL');
const buildPath = path.join(__dirname, 'bulid webgl');


// 정적 파일 서빙 설정
app.use(express.static(buildPath));

app.get('/test.html', (req, res) => {
  res.sendFile(path.join(buildPath, 'test.html'));
});

// 모든 요청을 index.html로 리디렉션
app.get('*', (req, res) => {
  res.sendFile(path.join(buildPath, 'index.html'));
});

app.listen(port, () => {
  console.log(`Server is running at http://localhost:${port}`);
  //console.log(`${buildPath}`);
});
