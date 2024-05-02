import './Header.css'
export default function Header(){
    return (
      <>
      <div className="header">
      <div className="logo">AutoSphere</div>
      <nav>
        <ul>
          <li><a href="index.html">Главная</a></li>
          <li><a href="shop.html">Магазин</a></li>
          <li><a href="contacts.html">Контакты</a></li>
        </ul>
      </nav>
      </div>
      </>
    )
  }