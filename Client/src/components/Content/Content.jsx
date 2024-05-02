import './Content.css'
export default function Content(props){
    return (
        <>
        <div class="Content">
              <h2>{props.title}</h2>
              <p>{props.description}</p>
          </div>
        </>
      )
}