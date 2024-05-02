import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import Header from './components/Header/Header'
import Footer from './components/Footer/Footer'
import Content from './components/Content/Content'
import { ContentData } from './data'
function App() {
  const [count, setCount] = useState(0)
  return (
    <>
      <Header />
    <main>
      <Content {...ContentData[0]}/>
      <Content {...ContentData[1]}/>
      <Content {...ContentData[2]}/>
      </main>
      <Footer />
    </>
  )
}
export default App
