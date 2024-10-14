import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';
import Header from './Header';
import Main from './Main';
import Footer from './Footer';
import '../styles/sign-up.css';

const SignUpForm: React.FC = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState(''); // 비밀번호 확인 상태 추가
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (password !== confirmPassword) {
      setError('비밀번호가 일치하지 않습니다.');
      return;
    }

    try {
      const response = await axios.post(
        `${process.env.REACT_APP_API_URL}/api/users/signup`,
        { username, password }
      );
      if (response.status === 201) {
        navigate('/login');
      }
    } catch (err) {
      setError('회원가입에 실패했습니다.');
    }
  };

  return (
    <div className="container">
      <Header />
      <Main children={undefined} />

        <section className="m">
          <h1 className="fade-in">Monster Killer</h1>
          <h2 className="fade-in">Hunt, Competition</h2>
          <p className="fade-in">"Monster Hunt"</p>
          <p className="fade-in famous">New monsters, New skills</p>
        </section>
      <form onSubmit={handleSubmit} className="signupForm">
        <h1 className="signup_title">회원가입</h1>
        <div className='signup_subtitle'>신규입력</div>
        <div className="signup_input">
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            placeholder="아이디"
            className="userId"
          />
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="비밀번호"
            className="password"
          />
         
        </div>
        {error && <p className="error">{error}</p>}
        <button type="submit" className="signup_button">회원가입</button>
        <Link to="/" className="loginLink">로그인</Link>
      </form>
      <Footer />
    </div>
  );
};

export default SignUpForm;



