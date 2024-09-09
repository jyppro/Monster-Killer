const express = require('express');
const { signup, login, getUserInfo, updatePassword } = require('../controllers/authController');

const router = express.Router();

router.post('/signup', signup);
router.post('/login', login);
router.get('/me', getUserInfo);
router.put('/me', updatePassword);

module.exports = router;