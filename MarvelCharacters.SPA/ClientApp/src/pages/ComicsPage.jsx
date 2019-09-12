import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import { ComicFactory } from '../factories/comicFactory';
import ComicCard from '../components/ComicCard';
import api from '../services/api';

const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1,
  },
  icon: {
    color: 'rgba(255, 255, 255, 0.54)',
  },
}));

export default function ComicsPage(props) {
  const [comics, setComics] = useState([]);

  async function handleLike(comic) {
    if(comic.liked)
      await api.delete(`/api/comics/${comic.id}/likes`, { "Accept": "application/json", "Content-Type": "application/json" });
    else
      await api.post(`/api/comics/${comic.id}/likes`, comic, { "Accept": "application/json", "Content-Type": "application/json" });

    await loadComics();
  }

  async function loadComics() {
    try
    {
      const response = await api.get(`/api/comics?searchString=${props.searchString}`)
      const data = response.data;
      const comicsColl = data.map(item => ComicFactory(item));
      setComics(comicsColl);
    }
    catch(error)
    {
      console.log(error);
    }
  }

  function handleShare(comic) {
    alert('nada implementado aqui');
  }

  useEffect(async () => {
    await loadComics();
  }, [props.searchString]);

  const classes = useStyles();
  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        {comics.length > 0 && comics.map((tile, index) => (
          <Grid item xs={2} key={index}>
            <ComicCard comic={tile} onLike={handleLike} onShare={handleShare} />
          </Grid>
        ))}
      </Grid>
    </div>
  );
}

