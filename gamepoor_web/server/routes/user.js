const express = require('express');
const router = express.Router();
const User = require('../models/userModel');
const bcrypt = require('bcryptjs');

// 회원가입 라우트
router.post('/signup', async (req, res) => {
  const { username, password } = req.body;

  if (!username || !password) {
    return res.status(400).json({ message: '아이디와 비밀번호를 입력하세요.' });
  }

  try {
    const hashedPassword = await bcrypt.hash(password, 10);
    const newUser = new User({ username, password: hashedPassword });

    await newUser.save();
    res.status(201).json({ message: '회원가입 성공' });
  } catch (error) {
    console.error('회원가입 오류:', error);
    if (error.code === 11000 && error.keyValue.username) {
      return res.status(400).json({ message: '이미 등록된 아이디입니다.' });
    }
    res.status(500).json({ message: '서버 오류' });
  }
});

// 로그인 라우트
router.post('/login', async (req, res) => {
  const { username, password } = req.body;

  if (!username || !password) {
    return res.status(400).json({ message: '아이디와 비밀번호를 입력하세요.' });
  }

  try {
    const user = await User.findOne({ username });

    if (!user) {
      return res.status(400).json({ success: false, message: '아이디 또는 비밀번호가 잘못되었습니다.' });
    }

    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) {
      return res.status(400).json({ success: false, message: '아이디 또는 비밀번호가 잘못되었습니다.' });
    }

    req.session.user = { _id: user._id, username: user.username };
    res.status(200).json({ success: true, message: '로그인 성공' });
  } catch (error) {
    res.status(500).json({ success: false, message: '서버 오류' });
  }
});

router.get('/me', (req, res) => {
  console.log('Session:', req.session);  // 세션 상태 로그
  if (!req.session.user) {
    return res.status(401).json({ message: '로그인이 필요합니다.' });
  }

  User.findById(req.session.user._id).select('-password')
    .then(user => {
      if (!user) {
        return res.status(404).json({ message: '사용자를 찾을 수 없습니다.' });
      }
      res.json({ username: user.username });
    })
    .catch(error => {
      console.error('서버 오류:', error);
      res.status(500).json({ message: '서버 오류' });
    });
});


// 비밀번호 수정 라우트
router.put('/me', async (req, res) => {
  if (!req.session.user) {
    return res.status(401).json({ message: '로그인이 필요합니다.' });
  }

  const { currentPassword, newPassword } = req.body;

  try {
    const user = await User.findById(req.session.user._id);
    if (!user) {
      return res.status(404).json({ message: '사용자를 찾을 수 없습니다.' });
    }

    const isMatch = await bcrypt.compare(currentPassword, user.password);
    if (!isMatch) {
      return res.status(400).json({ message: '현재 비밀번호가 잘못되었습니다.' });
    }

    if (newPassword) {
      user.password = await bcrypt.hash(newPassword, 10);
      await user.save();
      return res.json({ message: '비밀번호가 성공적으로 변경되었습니다.' });
    } else {
      return res.status(400).json({ message: '새로운 비밀번호를 입력하세요.' });
    }
  } catch (error) {
    res.status(500).json({ message: '서버 오류' });
  }
});

module.exports = router;
