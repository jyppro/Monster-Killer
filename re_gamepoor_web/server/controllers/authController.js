const User = require('../models/User');
const bcrypt = require('bcryptjs');  // bcrypt 대신 bcryptjs 사용

// Helper function for error responses
const sendErrorResponse = (res, status, message) => {
  res.status(status).json({ success: false, message });
};

// 회원가입 라우트
exports.signup = async (req, res) => {
  try {
    const { username, password } = req.body;

    if (!username || !password) {
      return sendErrorResponse(res, 400, '아이디와 비밀번호를 입력하세요.');
    }

    // 비밀번호 해싱
    const hashedPassword = await bcrypt.hash(password, 10);

    // 새로운 사용자 생성
    const user = new User({ username, password: hashedPassword });
    await user.save();

    res.status(201).json({ success: true, message: '회원가입 성공' });
  } catch (error) {
    console.error('회원가입 실패:', error);
    if (error.code === 11000 && error.keyValue.username) {
      return sendErrorResponse(res, 400, '이미 등록된 아이디입니다.');
    }
    sendErrorResponse(res, 500, '회원가입 실패');
  }
};

// 로그인 라우트
exports.login = async (req, res) => {
  const { username, password } = req.body;

  if (!username || !password) {
    return sendErrorResponse(res, 400, '아이디와 비밀번호를 입력하세요.');
  }

  try {
    const user = await User.findOne({ username });
    if (!user) {
      return sendErrorResponse(res, 400, '아이디 또는 비밀번호가 잘못되었습니다.');
    }

    const isMatch = await bcrypt.compare(password, user.password);
    if (!isMatch) {
      return sendErrorResponse(res, 400, '아이디 또는 비밀번호가 잘못되었습니다.');
    }

    req.session.user = { _id: user._id, username: user.username };
    res.status(200).json({ success: true, message: '로그인 성공' });
  } catch (error) {
    console.error('로그인 실패:', error);
    sendErrorResponse(res, 500, '로그인 실패');
  }
};

// 사용자 정보 조회 라우트
exports.getUserInfo = async (req, res) => {
  console.log('Session:', req.session); // 세션 상태 로그

  if (!req.session.user) {
    return sendErrorResponse(res, 401, '로그인이 필요합니다.');
  }

  try {
    const user = await User.findById(req.session.user._id).select('-password');
    if (!user) {
      return sendErrorResponse(res, 404, '사용자를 찾을 수 없습니다.');
    }
    res.status(200).json({ username: user.username });
  } catch (error) {
    console.error('서버 오류:', error);
    sendErrorResponse(res, 500, '서버 오류');
  }
};

// 비밀번호 수정 라우트
exports.updatePassword = async (req, res) => {
  if (!req.session.user) {
    return sendErrorResponse(res, 401, '로그인이 필요합니다.');
  }

  const { currentPassword, newPassword } = req.body;

  if (!newPassword) {
    return sendErrorResponse(res, 400, '새로운 비밀번호를 입력하세요.');
  }

  try {
    const user = await User.findById(req.session.user._id);
    if (!user) {
      return sendErrorResponse(res, 404, '사용자를 찾을 수 없습니다.');
    }

    const isMatch = await bcrypt.compare(currentPassword, user.password);
    if (!isMatch) {
      return sendErrorResponse(res, 400, '현재 비밀번호가 잘못되었습니다.');
    }

    user.password = await bcrypt.hash(newPassword, 10);
    await user.save();
    res.status(200).json({ success: true, message: '비밀번호가 성공적으로 변경되었습니다.' });
  } catch (error) {
    console.error('비밀번호 변경 오류:', error);
    sendErrorResponse(res, 500, '서버 오류');
  }
};