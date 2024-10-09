import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';
import Header from '../components/Header';
import Main from './Main';
import Footer from '../components/Footer';
import '../styles/login.css';
import '../styles/navbar.css';


const LoginForm: React.FC = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!username || !password) {
      setError('아이디와 비밀번호를 입력해 주세요.');
      return;
    }

    try {
      const response = await axios.post(
        `${process.env.REACT_APP_API_URL}/api/users/login`,
        { username, password },
        { withCredentials: true }
      );
      if (response.status === 200) {
        navigate('/game');
      }
    } catch (err) {
      setError('아이디 또는 비밀번호가 잘못되었습니다.');
    }
  };

  return (
    <div className="container">
      <Header />
      <Main>
      <section className="m">
          <h1 className="fade-in">몬스터 킬러</h1>
          <h2 className="fade-in">사냥, 경쟁</h2>
          <p className="fade-in">"몬스터 헌트"</p>
          <p className="fade-in famous">새로운 몬스터, 새로운 스킬</p>
        </section>
        <form onSubmit={handleSubmit} className="loginForm">
          <h1 className="login_title">로그인</h1>
          <div className="input">
            <input
              type="text"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="아이디"
              className="userId"
              aria-label="아이디"
            />
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="비밀번호"
              className="password"
              aria-label="비밀번호"
            />
          </div>
          {error && <p className="error">{error}</p>}
          <button type="submit" className="loginBut">로그인</button>
          <Link to="/signup" className="signupBut">회원가입</Link>
        </form>
      </Main>
      <Footer />
    </div>
  );
};

export default LoginForm;
