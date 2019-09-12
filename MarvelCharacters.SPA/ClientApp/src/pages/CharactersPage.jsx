import React, { useEffect, useState } from 'react';
import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import { CharacterFactory } from '../factories/characterFactory';
import CharacterCard from '../components/CharacterCard';
import api from '../services/api';

const useStyles = makeStyles(theme => ({
  root: {
    flexGrow: 1,
  },
  icon: {
    color: 'rgba(255, 255, 255, 0.54)',
  },
}));

export default function CharactersPage(props) {
  const [characters, setcharacters] = useState([]);

  async function handleLike(character) {
    if(character.liked)
      await api.delete(`/api/characters/${character.id}/likes`, { "Accept": "application/json", "Content-Type": "application/json" });
    else
      await api.post(`/api/characters/${character.id}/likes`, character, { "Accept": "application/json", "Content-Type": "application/json" });

    await loadCharacters();
  }

  function handleShare(character) {
    alert('nada implementado aqui');
  }

  async function loadCharacters() {
    try
    {
      const response = await api.get(`/api/characters?searchString=${props.searchString}`)
      const data = response.data;
      const charsColl = data.map(item => CharacterFactory(item));
      setcharacters(charsColl);
    }
    catch(error)
    {
      console.log(error);
    }
  }

  useEffect(() => {
    loadCharacters();
  }, [props.searchString]);

  const classes = useStyles();
  return (
    <div className={classes.root}>
      <Grid container spacing={3}>
        {characters.length > 0 && characters.map((tile, index) => (
          <Grid item xs={2} key={index}>
            <CharacterCard character={tile} onLike={handleLike} onShare={handleShare} />
          </Grid>
        ))}
      </Grid>
    </div>
  );
}

