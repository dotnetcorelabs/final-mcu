import t from 'prop-types';
import { ThumbnailFactory, thumbnailPropTypesSchema } from './thumbnailFactory';

function getShortDescription(description) {
    if(description)
        return description.substring(0, 36);
    return '';
}

export const ComicFactory = ({
    id = '',
    digitalId = '',
    title = '',
    description = '',
    thumbnail = ThumbnailFactory({}),
    liked = false
} = {}) => ({
    id: id,
    digitalId: digitalId,
    title: title,
    description: description,
    shortDescription: getShortDescription(description),
    thumbnail: ThumbnailFactory(thumbnail),
    liked: liked
});

export const comicsPropTypesSchema = t.shape({
    id: t.string.isRequired,
    digitalId: t.string.isRequired,
    title: t.string.isRequired,
    description: t.string,
    thumbnail: thumbnailPropTypesSchema.isRequired
});