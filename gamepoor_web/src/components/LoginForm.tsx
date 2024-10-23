import React, {useState} from 'react'
import {useNavigate, Link} from 'react-router-dom'
import Header from '../components/Header'
import Main from './Main'
import Footer from '../components/Footer'
import '../styles/login.css'
import '../styles/navbar.css'
import {ref, get, child} from 'firebase/database'
import {db} from '../FirebaseConfig'

const LoginForm: React.FC = () => {
  const [playerID, setPlayerID] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!playerID || !password) {
      setError('아이디와 비밀번호를 입력해 주세요.')
      return
    }

    try {
      const dbRef = ref(db)
      // const snapshot = await get(child(dbRef, `players/${'playerID_' + playerID}`))
      const snapshot = await get(child(dbRef, `players/${playerID}`))
      if (snapshot.exists()) {
        const playerData = snapshot.val()
        if (playerData.password === password) {
          navigate('/game', {state: {playerID}})
        } else {
          setError('아이디 또는 비밀번호가 잘못되었습니다.')
        }
      } else {
        setError('존재하지 않는 사용자입니다.')
      }
    } catch (err) {
      setError('로그인에 실패했습니다.')
      console.error('로그인 실패:', err)
    }
  }

  return (
    <div className="container">
      <Header />
      <Main>
        <section className="m">
          <h1 className="fade-in11">몬스터 킬러</h1>
          <h1 className="fade-in21">사냥, 경쟁</h1>
          <p className="fade-in21">"몬스터 헌트"</p>
          <p className="fade-in21">새로운 몬스터, 새로운 스킬</p>
        </section>
        <form onSubmit={handleSubmit} className="loginForm">
          <h1 className="login_title">로그인</h1>
          <div className="login_subtitle">계정 이름으로 로그인</div>
          <div className="input">
            <input
              type="text"
              value={playerID}
              onChange={e => setPlayerID(e.target.value)}
              placeholder="아이디"
              className="playerId"
              aria-label="아이디"
            />
            <input
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              placeholder="비밀번호"
              className="password"
              aria-label="비밀번호"
            />
          </div>
          {error && <p className="error">{error}</p>}
          <button type="submit" className="loginBut">
            로그인
          </button>
          <Link to="/signup" className="signupBut">
            회원가입
          </Link>
        </form>
      </Main>
      <Footer />
    </div>
  )
}

export default LoginForm
