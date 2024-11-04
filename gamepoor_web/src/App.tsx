import React from 'react'
import { Route, Routes } from 'react-router-dom'

import Main from './components/Main'
import LoginForm from './components/LoginForm'
import SignUpForm from './components/SignForm'
import { UnityPlayer } from './components/UnityPlayer'
import UserEditForm from './components/usereditform'
import Header from './components/Header'
import Overview from './components/Overview'
import Team from './components/Team'
import Mission from './components/Mission'
import Footer from './components/Footer'
import Screenshot1 from './components/Screenshot1'
import Screenshot2 from './components/Screenshot2'
import Screenshot3 from './components/Screenshot3'
import Events from './components/Events'
import Groups from './components/Groups'
import Forums from './components/Forums'
import FAQs from './components/Faqs'
import Contact from './components/Contact'
import Feedback from './components/Feedback'
import Ranking from './components/RankingPage'

const App: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Main><h1> </h1></Main>} />
      <Route path="/loginform" element={<LoginForm />} />
      <Route path="/header" element={<Header />} />
      <Route path="/overview" element={<Overview />} />
      <Route path="/team" element={<Team />} />
      <Route path="/mission" element={<Mission />} />
      <Route path="/footer" element={<Footer />} />
      <Route path="/signup" element={<SignUpForm />} />
      <Route path="/game" element={<UnityPlayer />} />
      <Route path="/edit" element={<UserEditForm />} />
      <Route path="/screenshot1" element={<Screenshot1 />} />
      <Route path="/screenshot2" element={<Screenshot2 />} />
      <Route path="/screenshot3" element={<Screenshot3 />} />
      <Route path="/events" element={<Events />} />
      <Route path="/groups" element={<Groups />} />
      <Route path="/forums" element={<Forums />} />
      <Route path="/faqs" element={<FAQs />} />
      <Route path="/contact" element={<Contact />} />
      <Route path="/feedback" element={<Feedback />} />
      <Route path="/ranking" element={<Ranking />} />
    </Routes>
  )
}

export default App
